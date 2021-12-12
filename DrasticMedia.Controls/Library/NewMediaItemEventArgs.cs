// <copyright file="NewMediaItemEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Model;

namespace DrasticMedia.Core.Library
{
    /// <summary>
    /// New Media Item Event Args.
    /// </summary>
    public class NewMediaItemEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="artistItem">AristItem.</param>
        public NewMediaItemEventArgs(ArtistItem artistItem)
        {
            this.MediaItem = artistItem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="trackItem">TrackItem.</param>
        public NewMediaItemEventArgs(TrackItem trackItem)
        {
            this.MediaItem = trackItem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="albumItem">AlbumItem.</param>
        public NewMediaItemEventArgs(AlbumItem albumItem)
        {
            this.MediaItem = albumItem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="tvShow">TVShow.</param>
        public NewMediaItemEventArgs(TVShow tvShow)
        {
            this.MediaItem = tvShow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="video">VideoItem.</param>
        public NewMediaItemEventArgs(VideoItem video)
        {
            this.MediaItem = video;
        }

        /// <summary>
        /// Gets the Media Item that was added.
        /// </summary>
        public object MediaItem { get; private set; }
    }
}
