// <copyright file="RemoveMediaItemEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Model;

namespace DrasticMedia.Core.Library
{
    /// <summary>
    /// Remove Media Item Event Args.
    /// </summary>
    public class RemoveMediaItemEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="artistItem">AristItem.</param>
        public RemoveMediaItemEventArgs(ArtistItem artistItem)
        {
            this.MediaItem = artistItem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="trackItem">TrackItem.</param>
        public RemoveMediaItemEventArgs(TrackItem trackItem)
        {
            this.MediaItem = trackItem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="albumItem">AlbumItem.</param>
        public RemoveMediaItemEventArgs(AlbumItem albumItem)
        {
            this.MediaItem = albumItem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="tvShow">TVShow.</param>
        public RemoveMediaItemEventArgs(TVShow tvShow)
        {
            this.MediaItem = tvShow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveMediaItemEventArgs"/> class.
        /// </summary>
        /// <param name="video">VideoItem.</param>
        public RemoveMediaItemEventArgs(VideoItem video)
        {
            this.MediaItem = video;
        }

        /// <summary>
        /// Gets the Media Item that was removed.
        /// </summary>
        public object MediaItem { get; private set; }
    }
}
