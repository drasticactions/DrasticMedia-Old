// <copyright file="TrackItem.cs" company="Drastic Actions">
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
    /// Track Item.
    /// </summary>
    public class TrackItem : MediaItem
    {
        /// <summary>
        /// Gets or sets the Id of the track.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ArtistItemId of the track.
        /// </summary>
        public int ArtistItemId { get; set; }

        /// <summary>
        /// Gets or sets the AlbumItemId of the track.
        /// </summary>
        public int AlbumItemId { get; set; }

        /// <summary>
        /// Gets or sets the last time this item was accessed.
        /// </summary>
        public DateTime LastAccessed { get; set; }

        /// <summary>
        /// Gets or sets the artist for the track.
        /// </summary>
        public virtual ArtistItem? ArtistItem { get; set; }

        /// <summary>
        /// Gets or sets the album for the track.
        /// </summary>
        public virtual AlbumItem? AlbumItem { get; set; }
    }
}
