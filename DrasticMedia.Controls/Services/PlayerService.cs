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

namespace DrasticMedia.Core.Services
{
    /// <summary>
    /// Player Service.
    /// </summary>
    public class PlayerService : INotifyPropertyChanged
    {
        private readonly IMediaService media;
        private readonly ILogger logger;
        private float currentPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerService"/> class.
        /// </summary>
        /// <param name="media"><see cref="IMediaService"/>.</param>
        /// <param name="logger"><see cref="ILogger"/>.</param>
        public PlayerService(IMediaService media, ILogger logger)
        {
            this.media = media;
            this.media.PositionChanged += this.Media_PositionChanged;
            this.logger = logger;
            this.Playlist = new List<MediaItem>();
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// New Media Added.
        /// </summary>
        public event EventHandler? NewMediaAdded;

        /// <summary>
        /// Is Playing Changed.
        /// </summary>
        public event EventHandler? IsPlayingChanged;

        /// <summary>
        /// The Media Service.
        /// </summary>
        public IMediaService MediaService => this.media;

        /// <summary>
        /// Gets or sets the current position of the current IMedia.
        /// </summary>
        public float CurrentPosition { get { return this.media.CurrentPosition; } set { this.media.CurrentPosition = value; } }

        /// <summary>
        /// Gets a value indicating whether the current IMedia is player.
        /// </summary>
        public bool IsPlaying => this.media.IsPlaying;

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

        private async Task SetAndPlayCurrentMedia(MediaItem media, bool fromPosition)
        {
            if (media == null)
            {
                throw new ArgumentNullException(nameof(media));
            }

            this.media.CurrentMedia = media;

            await this.media.PlayAsync(media.LastPosition, fromPosition);

            this.IsPlayingChanged?.Invoke(this, EventArgs.Empty);
        }

        private void Media_PositionChanged(object? sender, MediaPlayerPositionChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.CurrentPosition));
        }
    }
}
