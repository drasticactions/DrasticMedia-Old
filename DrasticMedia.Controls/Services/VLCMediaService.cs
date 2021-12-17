// <copyright file="VLCMediaService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DrasticMedia.Core.Model;
using LibVLCSharp.Shared;
using VLCPlayer = LibVLCSharp.Shared.MediaPlayer;

namespace DrasticMedia.Core.Services
{
    /// <summary>
    /// VLC Media Service.
    /// </summary>
    public class VLCMediaService : IMediaService
    {
        private VLCPlayer mediaPlayer;
        private Model.MediaItem? media;
        private LibVLCSharp.Shared.Media? vlcMedia;
        private LibVLC libVLC;

        /// <summary>
        /// Initializes a new instance of the <see cref="VLCMediaService"/> class.
        /// </summary>
        /// <param name="player">MediaPlayer.</param>
        /// <param name="libVLC">LibVLC.</param>
        public VLCMediaService(VLCPlayer player, LibVLC libVLC)
        {
            this.mediaPlayer = player;
            this.libVLC = libVLC;
            this.mediaPlayer.Playing += this.MediaPlayer_Playing;
            this.mediaPlayer.PositionChanged += this.MediaPlayer_PositionChanged;
            this.mediaPlayer.PausableChanged += this.MediaPlayer_PausableChanged;
            this.mediaPlayer.MediaChanged += this.MediaPlayer_MediaChanged;
            this.mediaPlayer.EndReached += this.MediaPlayer_EndReached;
        }

        /// <inheritdoc/>
        public event EventHandler<MediaPlayerPositionChangedEventArgs>? PositionChanged;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? EndCurrentItemReached;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? RaiseCanExecuteChanged;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? MediaChanged;

        /// <inheritdoc/>
        public Model.MediaItem? CurrentMedia { get { return media; } set { this.media = value; this.SetCurrentMedia(); } }

        /// <inheritdoc/>
        public bool IsPlaying => this.mediaPlayer.IsPlaying;

        /// <inheritdoc/>
        public float CurrentPosition { get { return this.mediaPlayer.Position; } set { this.mediaPlayer.Position = (float)value; } }

        /// <inheritdoc/>
        public Task PauseAsync()
        {
            this.mediaPlayer.Pause();
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task PlayAsync(double position = 0, bool fromPosition = false)
        {
            this.mediaPlayer.Play();
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task ResumeAsync()
        {
            this.mediaPlayer.Play();
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task SkipAhead(double amount = 0)
        {
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task SkipBack(double amount = 0)
        {
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task StopAsync()
        {
            this.mediaPlayer.Stop();
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<string> GetArtworkUrl() => this.GetMetadata(MetadataType.ArtworkURL);

        private void MediaPlayer_PositionChanged(object? sender, LibVLCSharp.Shared.MediaPlayerPositionChangedEventArgs e) 
            => this.PositionChanged?.Invoke(this, new MediaPlayerPositionChangedEventArgs(e.Position));

        private void MediaPlayer_PausableChanged(object? sender, MediaPlayerPausableChangedEventArgs e)
        {
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
        }

        private void MediaPlayer_Playing(object? sender, EventArgs e)
        {
            this.RaiseCanExecuteChanged?.Invoke(this, e);
        }

        private void MediaPlayer_EndReached(object? sender, EventArgs e)
        {
           this.EndCurrentItemReached?.Invoke(this, e);
        }

        private void MediaPlayer_MediaChanged(object? sender, MediaPlayerMediaChangedEventArgs e)
        {
            this.MediaChanged?.Invoke(this, e);
        }

        private void SetCurrentMedia()
        {
            if (this.CurrentMedia?.Path == null)
            {
                throw new NullReferenceException(nameof(this.CurrentMedia));
            }

            this.mediaPlayer.Stop();
            if (this.PathIsUrl(this.CurrentMedia.Path))
            {
                this.vlcMedia = new LibVLCSharp.Shared.Media(this.libVLC, this.CurrentMedia.Path, FromType.FromLocation);
            }
            else
            {
                this.vlcMedia = new LibVLCSharp.Shared.Media(this.libVLC, this.CurrentMedia.Path);
            }

            this.mediaPlayer.Media = this.vlcMedia;
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
        }

        private async Task<string> GetMetadata(MetadataType meta)
        {
            if (this.vlcMedia == null)
            {
                return string.Empty;
            }

            if (!this.vlcMedia.IsParsed)
            {
                await this.vlcMedia.Parse();
            }

            var stringUri = this.vlcMedia.Meta(meta);
            if (stringUri == null)
            {
                return string.Empty;
            }

            var uri = new Uri(stringUri);

            return HttpUtility.UrlDecode(uri.LocalPath) ?? string.Empty;
        }

        private bool PathIsUrl(string path)
        {
            if (File.Exists(path))
                return false;
            try
            {
                Uri uri = new Uri(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
