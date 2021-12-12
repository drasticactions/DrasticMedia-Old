// <copyright file="MediaItem.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticMedia.Core.Model
{
    /// <summary>
    /// Media File.
    /// </summary>
    public class MediaItem : IMediaItem
    {
        /// <summary>
        /// Gets or sets the path to the media.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the album.
        /// </summary>
        public string Album { get; set; }

        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Gets or sets the album artist.
        /// </summary>
        public string AlbumArtist { get; set; }

        /// <summary>
        /// Gets or sets the duration of the media.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the track number.
        /// </summary>
        public uint Tracknumber { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the genre.
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Gets or sets the path to the album art.
        /// </summary>
        public string AlbumArt { get; set; }

        /// <summary>
        /// Gets or sets the disc number.
        /// </summary>
        public int DiscNumber { get; set; }

        /// <summary>
        /// Gets or sets the season.
        /// </summary>
        public int Season { get; set; }

        /// <summary>
        /// Gets or sets the episode.
        /// </summary>
        public int Episode { get; set; }

        /// <summary>
        /// Gets or sets the episodes.
        /// </summary>
        public int Episodes { get; set; }

        /// <summary>
        /// Gets or sets the show title.
        /// </summary>
        public string ShowTitle { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        public string ThumbnailPath { get; set; }

        /// <summary>
        /// Gets or sets the poster path.
        /// </summary>
        public string PosterPath { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public uint Height { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public uint Width { get; set; }

        /// <summary>
        /// Gets or sets the amount of times this item has been played.
        /// </summary>
        public int PlayCount { get; set; }

        /// <summary>
        /// Gets or sets the last position of the item.
        /// </summary>
        public double LastPosition { get; set; }
    }
}
