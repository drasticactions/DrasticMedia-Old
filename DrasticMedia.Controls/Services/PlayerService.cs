// <copyright file="PlayerService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;

namespace DrasticMedia.Core.Services
{
    /// <summary>
    /// Player Service.
    /// </summary>
    public class PlayerService
    {
        private readonly IMediaService media;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerService"/> class.
        /// </summary>
        /// <param name="media"><see cref="IMediaService"/>.</param>
        /// <param name="logger"><see cref="ILogger"/>.</param>
        public PlayerService(IMediaService media, ILogger logger)
        {
            this.media = media;
            this.logger = logger;
            this.Playlist = new List<IMedia>();
        }

        /// <summary>
        /// New Media Added.
        /// </summary>
        public event EventHandler? NewMediaAdded;

        /// <summary>
        /// Is Playing Changed.
        /// </summary>
        public event EventHandler? IsPlayingChanged;

        /// <summary>
        /// Gets the current position of the current IMedia.
        /// </summary>
        public double CurrentPosition => this.media.CurrentPosition;

        /// <summary>
        /// Gets a value indicating whether the current IMedia is player.
        /// </summary>
        public bool IsPlaying => this.media.IsPlaying;

        /// <summary>
        /// Gets the current playlist.
        /// </summary>
        public IList<IMedia> Playlist { get; }

        /// <summary>
        /// Add Media to playlist.
        /// </summary>
        /// <param name="media">The media to add.</param>
        /// <param name="replaceCurrentItem">If we should replace the current item being played with this one.</param>
        /// <param name="location">The location to place the item. By default, it's added to the end. Throws if the location can't be set.</param>
        /// <param name="fromPosition">Play the item from the position.</param>
        /// <returns><see cref="Task"/>.</returns>
        /// <exception cref="ArgumentNullException">Media must be set.</exception>
        public async Task AddMedia(IMedia media, bool replaceCurrentItem, int location = -1, bool fromPosition = false)
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

        private async Task SetAndPlayCurrentMedia(IMedia media, bool fromPosition)
        {
            if (media == null)
            {
                throw new ArgumentNullException(nameof(media));
            }

            this.media.CurrentMedia = media;

            await this.media.PlayAsync(media.LastPosition, fromPosition);

            this.IsPlayingChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
