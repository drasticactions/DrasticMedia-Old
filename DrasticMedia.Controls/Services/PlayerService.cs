// <copyright file="PlayerService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;
using DrasticMedia.Services;
using DrasticMedia.Utilities;

namespace DrasticMedia.Core.Services
{
    /// <summary>
    /// Player Service.
    /// </summary>
    public class PlayerService : INotifyPropertyChanged
    {
        private readonly IErrorHandlerService error;
        private readonly IMediaService media;
        private readonly ILogger logger;
        private AsyncCommand? playPauseCommand;
        private AsyncCommand? goBackCommand;
        private AsyncCommand? goForwardCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerService"/> class.
        /// </summary>
        /// <param name="media"><see cref="IMediaService"/>.</param>
        /// <param name="error"><see cref="IErrorHandlerService"/>.</param>
        /// <param name="logger"><see cref="ILogger"/>.</param>
        public PlayerService(IMediaService media, IErrorHandlerService error, ILogger logger)
        {
            this.error = error;
            this.media = media;
            this.media.PositionChanged += this.Media_PositionChanged;
            this.media.RaiseCanExecuteChanged += Media_RaiseCanExecuteChanged;
            this.logger = logger;
            this.Playlist = new List<MediaItem>();
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// New Media Added.
        /// </summary>
        public event EventHandler? NewMediaAdded;

        /// <summary>
        /// Is Playing Changed.
        /// </summary>
        public event EventHandler? IsPlayingChanged;

        /// <summary>
        /// Gets the play pause command.
        /// </summary>
        public AsyncCommand PlayPauseCommand
        {
            get
            {
                return this.playPauseCommand ??= new AsyncCommand(this.PlayOrPause, () => this.media.CurrentMedia != null, this.error);
            }
        }

        /// <summary>
        /// Gets the go back command.
        /// </summary>
        public AsyncCommand GoBackCommand
        {
            get
            {
                return this.goBackCommand ??= new AsyncCommand(() => this.SetMediaItemFromIndex(this.Playlist.IndexOf(this.media.CurrentMedia) - 1), () => this.CanGoBack, this.error);
            }
        }

        /// <summary>
        /// Gets the go forward command.
        /// </summary>
        public AsyncCommand GoForwardCommand
        {
            get
            {
                return this.goForwardCommand ??= new AsyncCommand(() => this.SetMediaItemFromIndex(this.Playlist.IndexOf(this.media.CurrentMedia) + 1), () => this.CanGoForward, this.error);
            }
        }

        /// <summary>
        /// Gets The Media Service.
        /// </summary>
        public IMediaService MediaService => this.media;

        /// <summary>
        /// Gets or sets the current position of the current IMedia.
        /// </summary>
        public float CurrentPosition { get { return this.media.CurrentPosition; } set { this.media.CurrentPosition = value; } }

        /// <summary>
        /// Gets the current album art uri.
        /// </summary>
        public string CurrentAlbumArt => this.media.CurrentMedia?.AlbumArt ?? string.Empty;

        /// <summary>
        /// Gets the current artist.
        /// </summary>
        public string CurrentArtist => this.media.CurrentMedia?.Artist ?? string.Empty;

        /// <summary>
        /// Gets the current album title.
        /// </summary>
        public string CurrentAlbum => this.media.CurrentMedia?.Album ?? string.Empty;

        /// <summary>
        /// Gets the current title track.
        /// </summary>
        public string CurrentTrackTitle => this.media.CurrentMedia?.Title ?? string.Empty;

        /// <summary>
        /// Gets a value indicating whether the current IMedia is player.
        /// </summary>
        public bool IsPlaying => this.media.IsPlaying;

        /// <summary>
        /// Gets a value indicating whether the current media item is the first in the list.
        /// </summary>
        public bool CanGoBack => this.media.CurrentMedia != null && this.Playlist.IndexOf(this.media.CurrentMedia) > 0;

        /// <summary>
        /// Gets a value indicating whether the current media item is the first in the list.
        /// </summary>
        public bool CanGoForward => this.media.CurrentMedia != null && this.Playlist.IndexOf(this.media.CurrentMedia) < this.Playlist.Count - 1;

        /// <summary>
        /// Gets the current playlist.
        /// </summary>
        public IList<MediaItem> Playlist { get; }

        public Task PlayAsync(double position = 0, bool fromPosition = false) => this.media.PlayAsync(position, fromPosition);

        /// <summary>
        /// Add Media to playlist.
        /// </summary>
        /// <param name="media">The media to add.</param>
        /// <param name="replaceCurrentItem">If we should replace the current item being played with this one.</param>
        /// <param name="location">The location to place the item. By default, it's added to the end. Throws if the location can't be set.</param>
        /// <param name="fromPosition">Play the item from the position.</param>
        /// <returns><see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException">Media must be set.</exception>
        public async Task AddMedia(MediaItem media, bool replaceCurrentItem, int location = -1, bool fromPosition = false)
        {
            if (media == null)
            {
                throw new ArgumentNullException(nameof(media));
            }

            if (location > 0)
            {
                try
                {
                    this.Playlist.Insert(location, media);
                }
                catch (Exception ex)
                {
                    // TODO: Handle Exception.
                    this.logger.Log(ex, LogLevel.Error);
                    throw;
                }
            }
            else
            {
                this.Playlist.Add(media);
            }

            if (replaceCurrentItem)
            {
                await this.SetAndPlayCurrentMedia(media, fromPosition);
            }
        }

        /// <summary>
        /// Called when wanting to raise a Command Can Execute.
        /// </summary>
        public virtual void RaiseCanExecuteChanged()
        {
            this.PlayPauseCommand?.RaiseCanExecuteChanged();
            this.GoBackCommand?.RaiseCanExecuteChanged();
            this.GoForwardCommand?.RaiseCanExecuteChanged();
            this.OnPropertyChanged(nameof(this.IsPlaying));
            this.OnPropertyChanged(nameof(this.CurrentAlbumArt));
            this.OnPropertyChanged(nameof(this.CurrentArtist));
            this.OnPropertyChanged(nameof(this.CurrentAlbum));
            this.OnPropertyChanged(nameof(this.CurrentTrackTitle));
        }

#pragma warning disable SA1600 // Elements should be documented
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
#pragma warning restore SA1600 // Elements should be documented
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// On Property Changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    var changed = this.PropertyChanged;
                    if (changed == null)
                    {
                        return;
                    }

                    changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
                });
            }
        }

        private async Task PlayOrPause()
        {
            if (this.IsPlaying)
            {
                await this.media.PauseAsync();
            }
            else
            {
                await this.media.ResumeAsync();
            }
        }

        private async Task SetMediaItemFromIndex(int index)
        {
            try
            {
                var playlistItem = this.Playlist[index];
                await this.SetAndPlayCurrentMedia(playlistItem, false);
            }
            catch (Exception ex)
            {
                this.logger.Log(ex);
            }
        }

        private async Task SetAndPlayCurrentMedia(MediaItem media, bool fromPosition)
        {
            if (media == null)
            {
                throw new ArgumentNullException(nameof(media));
            }

            this.media.CurrentMedia = media;

            await this.media.PlayAsync(media.LastPosition, fromPosition);

            this.IsPlayingChanged?.Invoke(this, EventArgs.Empty);

            this.RaiseCanExecuteChanged();
        }

        private void Media_PositionChanged(object? sender, MediaPlayerPositionChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.CurrentPosition));
        }

        private void Media_RaiseCanExecuteChanged(object? sender, EventArgs e)
        {
            this.RaiseCanExecuteChanged();
        }
    }
}
