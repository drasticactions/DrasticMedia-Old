// <copyright file="UpdateMediaItemEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Model;

namespace DrasticMedia.Core.Library
{
    /// <summary>
    /// Update Media Item Event Args.
    /// </summary>
    public class UpdateMediaItemEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="artistItem">AristItem.</param>
        public UpdateMediaItemEventArgs(ArtistItem artistItem)
        {
            this.MediaItem = artistItem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="trackItem">TrackItem.</param>
        public UpdateMediaItemEventArgs(TrackItem trackItem)
        {
            this.MediaItem = trackItem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="albumItem">AlbumItem.</param>
        public UpdateMediaItemEventArgs(AlbumItem albumItem)
        {
            this.MediaItem = albumItem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="tvShow">TVShow.</param>
        public UpdateMediaItemEventArgs(TVShow tvShow)
        {
            this.MediaItem = tvShow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="video">VideoItem.</param>
        public UpdateMediaItemEventArgs(VideoItem video)
        {
            this.MediaItem = video;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="podcast">PodcastShowItem.</param>
        public UpdateMediaItemEventArgs(PodcastShowItem podcast)
        {
            this.MediaItem = podcast;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="podcast">PodcastEpisodeItem.</param>
        public UpdateMediaItemEventArgs(PodcastEpisodeItem podcast)
        {
            this.MediaItem = podcast;
        }

        /// <summary>
        /// Gets the Media Item that was added.
        /// </summary>
        public object MediaItem { get; private set; }
    }
}