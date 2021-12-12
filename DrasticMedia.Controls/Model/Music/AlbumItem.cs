// <copyright file="AlbumItem.cs" company="Drastic Actions">
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
    /// Album Item.
    /// </summary>
    public class AlbumItem
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
        /// Gets or sets the name of the artist.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets a list of tracks on this album.
        /// </summary>
        public virtual List<TrackItem>? Tracks { get; set; }

        /// <summary>
        /// Gets or sets the last time this item was accessed.
        /// </summary>
        public DateTime LastAccessed { get; set; }

        /// <summary>
        /// Gets or sets the artist for the album.
        /// </summary>
        public virtual ArtistItem? ArtistItem { get; set; }
    }
}
