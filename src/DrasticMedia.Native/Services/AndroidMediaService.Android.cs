// <copyright file="AndroidMediaService.Android.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.Media.Session;
using Android.Net;
using Android.Net.Wifi;
using Android.OS;
using DrasticMedia.Core.Recievers;
using AndroidNet = Android.Net;

namespace DrasticMedia.Core.Services
{
    /// <summary>
    /// Android Media Player Service.
    /// </summary>
    [Service]
    [IntentFilter(new[] { ActionPlay, ActionPause, ActionStop, ActionTogglePlayback, ActionNext, ActionPrevious })]
#pragma warning disable SA1649 // File name should match first type name
    public class MediaPlayerService : Service,
#pragma warning restore SA1649 // File name should match first type name
           AudioManager.IOnAudioFocusChangeListener,
           MediaPlayer.IOnBufferingUpdateListener,
           MediaPlayer.IOnCompletionListener,
           MediaPlayer.IOnErrorListener,
           MediaPlayer.IOnPreparedListener
    {
        // Actions

        /// <summary>
        /// Action Player.
        /// </summary>
        public const string ActionPlay = "com.xamarin.action.PLAY";

        /// <summary>
        /// Action Pause.
        /// </summary>
        public const string ActionPause = "com.xamarin.action.PAUSE";

        /// <summary>
        /// Action Stop.
        /// </summary>
        public const string ActionStop = "com.xamarin.action.STOP";

        /// <summary>
        /// Action Toggle Playback.
        /// </summary>
        public const string ActionTogglePlayback = "com.xamarin.action.TOGGLEPLAYBACK";

        /// <summary>
        /// Action Next.
        /// </summary>
        public const string ActionNext = "com.xamarin.action.NEXT";

        /// <summary>
        /// Action Prevous.
        /// </summary>
        public const string ActionPrevious = "com.xamarin.action.PREVIOUS";

        public MediaPlayer? MediaPlayer;

        public Android.Media.Session.MediaController? MediaController;

        private AudioManager? audioManager;

        private MediaSession? mediaSession;

        private int buffered = 0;

        private WifiManager? wifiManager;
        private WifiManager.WifiLock? wifiLock;

        /// <summary>
        /// Status Changed.
        /// </summary>
        public event StatusChangedEventHandler? StatusChanged;

        /// <summary>
        /// Cover Reloaded.
        /// </summary>
        public event CoverReloadedEventHandler? CoverReloaded;

        /// <summary>
        /// Is Playing.
        /// </summary>
        public event PlayingEventHandler? Playing;

        /// <summary>
        /// Is Buffering.
        /// </summary>
        public event BufferingEventHandler? Buffering;

        private readonly Handler playingHandler;
        private readonly Java.Lang.Runnable playingHandlerRunnable;

        private ComponentName? remoteComponentName;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlayerService"/> class.
        /// </summary>
        public MediaPlayerService()
        {
            if (Looper.MainLooper is null)
            {
                throw new NullReferenceException(nameof(Looper));
            }

            this.playingHandler = new Handler(Looper.MainLooper);

            // Create a runnable, restarting itself if the status still is "playing"
            this.playingHandlerRunnable = new Java.Lang.Runnable(() =>
            {
                this.OnPlaying(EventArgs.Empty);

                if (this.MediaPlayerState == PlaybackStateCode.Playing)
                {
                    if (this.playingHandlerRunnable is not null)
                    {
                        this.playingHandler.PostDelayed(this.playingHandlerRunnable, 250);
                    }
                }
            });

            // On Status changed to PLAYING, start raising the Playing event
            this.StatusChanged += (object sender, EventArgs e) =>
            {
                if (this.MediaPlayerState == PlaybackStateCode.Playing)
                {
                    this.playingHandler.PostDelayed(this.playingHandlerRunnable, 0);
                }
            };
        }

        /// <summary>
        /// Gets the Media Player State.
        /// </summary>
        public PlaybackStateCode MediaPlayerState
        {
            get
            {
                return this.MediaController?.PlaybackState != null
                    ? this.MediaController.PlaybackState.State
                    : PlaybackStateCode.None;
            }
        }

        /// <summary>
        /// On Status Changed.
        /// </summary>
        /// <param name="e">Event Args.</param>
        protected virtual void OnStatusChanged(EventArgs e)
        {
            this.StatusChanged?.Invoke(this, e);
        }

        /// <summary>
        /// On Cover Reloaded.
        /// </summary>
        /// <param name="e">Event Args.</param>
        protected virtual void OnCoverReloaded(EventArgs e)
        {
            if (this.CoverReloaded != null)
            {
                this.CoverReloaded(this, e);
                this.StartNotification();
                this.UpdateMediaMetadataCompat();
            }
        }

        /// <summary>
        /// On Playing.
        /// </summary>
        /// <param name="e">Event Args.</param>
        protected virtual void OnPlaying(EventArgs e)
        {
            this.Playing?.Invoke(this, e);
        }

        /// <summary>
        /// On Buffering.
        /// </summary>
        /// <param name="e">Event Args.</param>
        protected virtual void OnBuffering(EventArgs e)
        {
            this.Buffering?.Invoke(this, e);
        }

        /// <summary>
        /// On create simply detect some of our managers
        /// </summary>
        public override void OnCreate()
        {
            base.OnCreate();

            // Find our audio and notificaton managers
            var audioManager = this.GetSystemService(AudioService) as AudioManager;
            if (audioManager is not null)
            {
                this.audioManager = audioManager;
            }

            var wifiManager = this.GetSystemService(WifiService) as WifiManager;
            if (wifiManager is not null)
            {
                this.wifiManager = wifiManager;
            }

            if (this.PackageName is not null)
            {
                this.remoteComponentName = new ComponentName(this.PackageName, new RemoteControlBroadcastReceiver().ComponentName);
            }
        }

        /// <summary>
        /// On Buffering Update.
        /// </summary>
        /// <param name="mp">Media Player.</param>
        /// <param name="percent">Precent Buffered.</param>
        public void OnBufferingUpdate(MediaPlayer? mp, int percent)
        {
            if (mp is null)
            {
                return;
            }

            int duration = 0;
            if (this.MediaPlayerState == PlaybackStateCode.Playing || this.MediaPlayerState == PlaybackStateCode.Paused)
            {
                duration = mp.Duration;
            }

            int newBufferedTime = duration * percent / 100;
            if (newBufferedTime != this.Buffered)
            {
                this.Buffered = newBufferedTime;
            }
        }

        /// <summary>
        /// On Completion.
        /// </summary>
        /// <param name="mp">Media Player.</param>
        public async void OnCompletion(MediaPlayer? mp)
        {
            await this.PlayNext();
        }

        /// <summary>
        /// On Error.
        /// </summary>
        /// <param name="mp">Media Player.</param>
        /// <param name="what">The Error.</param>
        /// <param name="extra">Extra.</param>
        /// <returns>Bool.</returns>
        public bool OnError(MediaPlayer? mp, MediaError what, int extra)
        {
            this.UpdatePlaybackState(PlaybackStateCode.Error);
            return true;
        }

        /// <summary>
        /// On Prepared.
        /// </summary>
        /// <param name="mp">Media Player.</param>
        public void OnPrepared(MediaPlayer? mp)
        {
            mp?.Start();
            this.UpdatePlaybackState(PlaybackStateCode.Playing);
        }

        public string? AudioUrl;

        /// <summary>
        /// Gets the position.
        /// </summary>
        public int Position
        {
            get
            {
                if (this.MediaPlayer == null
                    || (this.MediaPlayerState != PlaybackStateCode.Playing
                        && this.MediaPlayerState != PlaybackStateCode.Paused))
                {
                    return -1;
                }
                else
                {
                    return this.MediaPlayer.CurrentPosition;
                }
            }
        }

        /// <summary>
        /// Gets the duration.
        /// </summary>
        public int Duration
        {
            get
            {
                if (this.MediaPlayer == null
                    || (this.MediaPlayerState != PlaybackStateCode.Playing
                        && this.MediaPlayerState != PlaybackStateCode.Paused))
                {
                    return 0;
                }
                else
                {
                    return this.MediaPlayer.Duration;
                }
            }
        }

        public int Buffered
        {
            get
            {
                if (this.MediaPlayer == null)
                {
                    return 0;
                }
                else
                {
                    return this.buffered;
                }
            }

            private set
            {
                this.buffered = value;
                this.OnBuffering(EventArgs.Empty);
            }
        }

        private Bitmap? cover;

        public object? Cover
        {
            get
            {
                return this.cover;
            }

            private set
            {
                this.cover = value as Bitmap;
                this.OnCoverReloaded(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Intializes the player.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Play()
        {
            if (this.MediaPlayer != null && this.MediaPlayerState == PlaybackStateCode.Paused)
            {
                // We are simply paused so just start again
                this.MediaPlayer.Start();
                this.UpdatePlaybackState(PlaybackStateCode.Playing);
                this.StartNotification();

                // Update the metadata now that we are playing
                this.UpdateMediaMetadataCompat();
                return;
            }

            if (this.MediaPlayer == null)
            {
                this.InitializePlayer();
            }

            if (this.mediaSession == null)
            {
                this.InitMediaSession();
            }

            if (this.MediaPlayer is null)
            {
                return;
            }

            if (this.MediaPlayer.IsPlaying)
            {
                this.UpdatePlaybackState(PlaybackStateCode.Playing);
                return;
            }

            await this.PrepareAndPlayMediaPlayerAsync();
        }

        private async Task PrepareAndPlayMediaPlayerAsync()
        {
            try
            {
                MediaMetadataRetriever metaRetriever = new MediaMetadataRetriever();
                var audioUrl = AndroidNet.Uri.Parse(this.AudioUrl);

                if (this.ApplicationContext is not null && this.MediaPlayer is not null && audioUrl is not null)
                {
                    await this.MediaPlayer.SetDataSourceAsync(this.ApplicationContext, audioUrl);
                }

                await metaRetriever.SetDataSourceAsync(this.AudioUrl, new Dictionary<string, string>());

                var audioFocus = new AudioFocusRequestClass
                    .Builder(AudioFocus.Gain)
                    .SetOnAudioFocusChangeListener(this)
                    .Build();

                AudioFocusRequest? focusResult = null;
                if (audioFocus is not null)
                {
                    focusResult = this.audioManager?.RequestAudioFocus(audioFocus);
                }

                if (focusResult != AudioFocusRequest.Granted)
                {
                    // Could not get audio focus
                    Console.WriteLine("Could not get audio focus");
                }

                this.UpdatePlaybackState(PlaybackStateCode.Buffering);
                this.MediaPlayer?.PrepareAsync();

                this.AquireWifiLock();
                this.UpdateMediaMetadataCompat(metaRetriever);
                this.StartNotification();

                byte[] imageByteArray = metaRetriever?.GetEmbeddedPicture() ?? new byte[0];
                this.Cover = await BitmapFactory.DecodeByteArrayAsync(imageByteArray, 0, imageByteArray.Length);
            }
            catch (Exception ex)
            {
                this.UpdatePlaybackState(PlaybackStateCode.Stopped);

                if (this.MediaPlayer is not null)
                {
                    this.MediaPlayer.Reset();
                    this.MediaPlayer.Release();
                    this.MediaPlayer = null;
                }

                // Unable to start playback log error
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Seek.
        /// </summary>
        /// <param name="position">Position to seek.</param>
        /// <returns>Task.</returns>
        public async Task Seek(int position)
        {
            await Task.Run(() =>
            {
                if (this.MediaPlayer != null)
                {
                    this.MediaPlayer.SeekTo(position);
                }
            });
        }

        /// <summary>
        /// Play Next.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task PlayNext()
        {
            if (this.MediaPlayer != null)
            {
                this.MediaPlayer.Reset();
                this.MediaPlayer.Release();
                this.MediaPlayer = null;
            }

            this.UpdatePlaybackState(PlaybackStateCode.SkippingToNext);

            await this.Play();
        }

        /// <summary>
        /// Play Pause.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task PlayPrevious()
        {
            // Start current track from beginning if it's the first track or the track has played more than 3sec and you hit "playPrevious".
            if (this.Position > 3000)
            {
                await this.Seek(0);
            }
            else
            {
                if (this.MediaPlayer != null)
                {
                    this.MediaPlayer.Reset();
                    this.MediaPlayer.Release();
                    this.MediaPlayer = null;
                }

                this.UpdatePlaybackState(PlaybackStateCode.SkippingToPrevious);

                await this.Play();
            }
        }

        /// <summary>
        /// Play Pause.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task PlayPause()
        {
            if (this.MediaPlayer == null || (this.MediaPlayer != null && this.MediaPlayerState == PlaybackStateCode.Paused))
            {
                await this.Play();
            }
            else
            {
                await this.Pause();
            }
        }

        /// <summary>
        /// Pause Media Player.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Pause()
        {
            await Task.Run(() =>
            {
                if (this.MediaPlayer == null)
                {
                    return;
                }

                if (this.MediaPlayer.IsPlaying)
                {
                    this.MediaPlayer.Pause();
                }

                this.UpdatePlaybackState(PlaybackStateCode.Paused);
            });
        }

        /// <summary>
        /// Stop Media Player.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Stop()
        {
            await Task.Run(() =>
            {
                if (this.MediaPlayer == null)
                {
                    return;
                }

                if (this.MediaPlayer.IsPlaying)
                {
                    this.MediaPlayer.Stop();
                }

                this.UpdatePlaybackState(PlaybackStateCode.Stopped);
                this.MediaPlayer.Reset();
                if (this.ApplicationContext is not null)
                {
                    NotificationHelper.StopNotification(this.ApplicationContext);
                }

                this.StopForeground(true);
                this.ReleaseWifiLock();
                this.UnregisterMediaSessionCompat();
            });
        }

        /// <inheritdoc/>
        public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
        {
            this.HandleIntent(intent);
            return base.OnStartCommand(intent, flags, startId);
        }

        private void UpdatePlaybackState(PlaybackStateCode state)
        {
            if (this.mediaSession == null || this.MediaPlayer == null)
                return;

            try
            {
#pragma warning disable CS8602
                PlaybackState.Builder? stateBuilder = new PlaybackState.Builder()
                    .SetActions(
                        PlaybackState.ActionPause |
                        PlaybackState.ActionPlay |
                        PlaybackState.ActionPlayPause |
                        PlaybackState.ActionSkipToNext |
                        PlaybackState.ActionSkipToPrevious |
                        PlaybackState.ActionStop
                    )
                    .SetState(state, this.Position, 1.0f, SystemClock.ElapsedRealtime());
#pragma warning restore CS8602

                if (stateBuilder is null)
                {
                    return;
                }

                this.mediaSession.SetPlaybackState(stateBuilder.Build());

                this.OnStatusChanged(EventArgs.Empty);

                if (state == PlaybackStateCode.Playing || state == PlaybackStateCode.Paused)
                {
                    this.StartNotification();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void StartNotification()
        {
            if (this.mediaSession == null)
            {
                return;
            }

            if (this.ApplicationContext is not null && this.MediaController?.Metadata is not null && this.Cover is not null)
            {
                NotificationHelper.StartNotification(
                this.ApplicationContext,
                this.MediaController.Metadata,
                this.mediaSession,
                this.Cover,
                this.MediaPlayerState == PlaybackStateCode.Playing);
            }
        }

        /// <summary>
        /// Updates the metadata on the lock screen.
        /// </summary>
        private void UpdateMediaMetadataCompat(MediaMetadataRetriever? metaRetriever = null)
        {
            if (this.mediaSession == null)
            {
                return;
            }

            MediaMetadata.Builder builder = new MediaMetadata.Builder();
#pragma warning disable CS8602
            if (metaRetriever != null)
            {
                builder
                .PutString(MediaMetadata.MetadataKeyAlbum, metaRetriever.ExtractMetadata(MetadataKey.Album))
                .PutString(MediaMetadata.MetadataKeyArtist, metaRetriever.ExtractMetadata(MetadataKey.Artist))
                .PutString(MediaMetadata.MetadataKeyTitle, metaRetriever.ExtractMetadata(MetadataKey.Title));

            }
            else
            {
                builder
                    .PutString(MediaMetadata.MetadataKeyAlbum, mediaSession.Controller.Metadata.GetString(MediaMetadata.MetadataKeyAlbum))
                    .PutString(MediaMetadata.MetadataKeyArtist, mediaSession.Controller.Metadata.GetString(MediaMetadata.MetadataKeyArtist))
                    .PutString(MediaMetadata.MetadataKeyTitle, mediaSession.Controller.Metadata.GetString(MediaMetadata.MetadataKeyTitle));
            }
#pragma warning restore CS8602
            builder.PutBitmap(MediaMetadata.MetadataKeyAlbumArt, this.Cover as Bitmap);

            this.mediaSession.SetMetadata(builder.Build());
        }

        private void HandleIntent(Intent? intent)
        {
            if (intent == null || intent.Action == null)
            {
                return;
            }

            string action = intent.Action;

            if (action.Equals(ActionPlay))
            {
                this.MediaController?.GetTransportControls().Play();
            }
            else if (action.Equals(ActionPause))
            {
                this.MediaController?.GetTransportControls().Pause();
            }
            else if (action.Equals(ActionPrevious))
            {
                this.MediaController?.GetTransportControls().SkipToPrevious();
            }
            else if (action.Equals(ActionNext))
            {
                this.MediaController?.GetTransportControls().SkipToNext();
            }
            else if (action.Equals(ActionStop))
            {
                this.MediaController?.GetTransportControls().Stop();
            }
        }

        /// <summary>
        /// Lock the wifi so we can still stream under lock screen.
        /// </summary>
        private void AquireWifiLock()
        {
            if (this.wifiLock == null)
            {
                this.wifiLock = this.wifiManager?.CreateWifiLock(WifiMode.Full, "xamarin_wifi_lock");
            }

            this.wifiLock?.Acquire();
        }

        /// <summary>
        /// This will release the wifi lock if it is no longer needed.
        /// </summary>
        private void ReleaseWifiLock()
        {
            if (this.wifiLock == null)
                return;

            this.wifiLock.Release();
            this.wifiLock = null;
        }

        /// <summary>
        /// Will register for the remote control client commands in audio manager.
        /// </summary>
        private void InitMediaSession()
        {
            try
            {
                if (this.mediaSession == null)
                {
                    if (this.ApplicationContext is null)
                    {
                        return;
                    }

                    Intent nIntent = new Intent(this.ApplicationContext, typeof(Activity));

                    if (this.PackageName is not null)
                    {
                        this.remoteComponentName = new ComponentName(this.PackageName, new RemoteControlBroadcastReceiver().ComponentName);
                    }

                    this.mediaSession = new MediaSession(this.ApplicationContext, "DrasticStreamingAudio"/*, remoteComponentName*/); // TODO
                    this.mediaSession.SetSessionActivity(PendingIntent.GetActivity(this.ApplicationContext, 0, nIntent, 0));
                    this.MediaController = new Android.Media.Session.MediaController(this.ApplicationContext, this.mediaSession.SessionToken);
                    this.mediaSession.Active = true;
                    if (this.binder is not null)
                    {
                        this.mediaSession.SetCallback(new MediaSessionCallback((MediaPlayerServiceBinder)this.binder));
                    }

                    this.mediaSession.SetFlags(MediaSessionFlags.HandlesMediaButtons | MediaSessionFlags.HandlesTransportControls);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Intializes the player.
        /// </summary>
        private void InitializePlayer()
        {
            this.MediaPlayer = new MediaPlayer();

#pragma warning disable CS8602
            var audioAttributes = new AudioAttributes.Builder()
                .SetContentType(AudioContentType.Music)
                .SetUsage(AudioUsageKind.Media)
                    .Build();
#pragma warning restore CS8602

            if (audioAttributes is null)
            {
                return;
            }

            this.MediaPlayer.SetAudioAttributes(audioAttributes);
            this.MediaPlayer.SetWakeMode(this.ApplicationContext, WakeLockFlags.Partial);
            this.MediaPlayer.SetOnBufferingUpdateListener(this);
            this.MediaPlayer.SetOnCompletionListener(this);
            this.MediaPlayer.SetOnErrorListener(this);
            this.MediaPlayer.SetOnPreparedListener(this);
        }

        private void UnregisterMediaSessionCompat()
        {
            try
            {
                if (this.mediaSession != null)
                {
                    this.mediaSession.Dispose();
                    this.mediaSession = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        IBinder? binder;

        public override IBinder OnBind(Intent? intent)
        {
            this.binder = new MediaPlayerServiceBinder(this);
            return this.binder;
        }

        public override bool OnUnbind(Intent? intent)
        {
            if (this.ApplicationContext is null)
            {
                return false;
            }

            NotificationHelper.StopNotification(ApplicationContext);
            return base.OnUnbind(intent);
        }

        /// <summary>
        /// Properly cleanup of your player by releasing resources
        /// </summary>
        public override void OnDestroy()
        {
            base.OnDestroy();
            if (this.MediaPlayer != null)
            {
                this.MediaPlayer.Release();
                this.MediaPlayer = null;

                if (this.ApplicationContext is null)
                {
                    return;
                }

                NotificationHelper.StopNotification(this.ApplicationContext);
                this.StopForeground(true);
                this.ReleaseWifiLock();
                this.UnregisterMediaSessionCompat();
            }
        }

        public async void OnAudioFocusChange(AudioFocus focusChange)
        {
            if (this.MediaPlayer is null)
            {
                return;
            }

            switch (focusChange)
            {
                case AudioFocus.Gain:
                    if (this.MediaPlayer == null)
                    {
                        this.InitializePlayer();
                    }

                    if (this.MediaPlayer is null)
                    {
                        throw new NullReferenceException(nameof(this.MediaPlayer));
                    }

                    if (!this.MediaPlayer.IsPlaying)
                    {
                        this.MediaPlayer.Start();
                    }

                    this.MediaPlayer.SetVolume(1.0f, 1.0f);
                    break;
                case AudioFocus.Loss:
                    // We have lost focus stop!
                    await this.Stop();
                    break;
                case AudioFocus.LossTransient:
                    // We have lost focus for a short time, but likely to resume so pause
                    await this.Pause();
                    break;
                case AudioFocus.LossTransientCanDuck:
                    // We have lost focus but should till play at a muted 10% volume
                    if (this.MediaPlayer.IsPlaying)
                    {
                        this.MediaPlayer.SetVolume(.1f, .1f);
                    }

                    break;
            }
        }

        /// <summary>
        /// Media Session Callback.
        /// </summary>
        public class MediaSessionCallback : MediaSession.Callback
        {
            private readonly MediaPlayerServiceBinder mediaPlayerService;

            /// <summary>
            /// Initializes a new instance of the <see cref="MediaSessionCallback"/> class.
            /// </summary>
            /// <param name="service">Media Player Service Binder.</param>
            public MediaSessionCallback(MediaPlayerServiceBinder service)
            {
                this.mediaPlayerService = service;
            }

            /// <inheritdoc/>
            public override async void OnPause()
            {
                await this.mediaPlayerService.GetMediaPlayerService().Pause();
                base.OnPause();
            }

            /// <inheritdoc/>
            public override async void OnPlay()
            {
                await this.mediaPlayerService.GetMediaPlayerService().Play();
                base.OnPlay();
            }

            /// <inheritdoc/>
            public override async void OnSkipToNext()
            {
                await this.mediaPlayerService.GetMediaPlayerService().PlayNext();
                base.OnSkipToNext();
            }

            /// <inheritdoc/>
            public override async void OnSkipToPrevious()
            {
                await this.mediaPlayerService.GetMediaPlayerService().PlayPrevious();
                base.OnSkipToPrevious();
            }

            /// <inheritdoc/>
            public override async void OnStop()
            {
                await this.mediaPlayerService.GetMediaPlayerService().Stop();
                base.OnStop();
            }
        }
    }

    /// <summary>
    /// Status Changed Event Handler.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event Args.</param>
    public delegate void StatusChangedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Buffering Event Handler.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event Args.</param>
    public delegate void BufferingEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Cover Reloaded Event.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event Args.</param>
    public delegate void CoverReloadedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Playing Event Handler.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event Args.</param>
    public delegate void PlayingEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Media Player Service Binder.
    /// </summary>
    public class MediaPlayerServiceBinder : Binder
    {
        private readonly MediaPlayerService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlayerServiceBinder"/> class.
        /// </summary>
        /// <param name="service">Media Player Service.</param>
        public MediaPlayerServiceBinder(MediaPlayerService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets the media player service.
        /// </summary>
        /// <returns></returns>
        public MediaPlayerService GetMediaPlayerService()
        {
            return this.service;
        }
    }
}
