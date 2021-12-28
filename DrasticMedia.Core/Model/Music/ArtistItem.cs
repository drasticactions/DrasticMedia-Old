// <copyright file="ArtistItem.cs" company="Drastic Actions">
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
    /// Artist Item.
    /// </summary>
    public class ArtistItem
    {
        /// <summary>
        /// Gets or sets the Id of the artist.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the MusicBrainz id.
        /// </summary>
        public string? MBID { get; set; }

        /// <summary>
        /// Gets or sets the Spotify Id.
        /// </summary>
        public string? SpotifyId { get; set; }

        /// <summary>
        /// Gets or sets the name of the artist.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the artist biography.
        /// </summary>
        public string? Biography { get; set; }

        /// <summary>
        /// Gets or sets the path to the image of the artist.
        /// </summary>
        public string? ArtistImage { get; set; }

        /// <summary>
        /// Gets or sets a list of albums by this artist.
        /// </summary>
        public virtual List<AlbumItem> Albums { get; set; } = new List<AlbumItem>();

        /// <summary>
        /// Gets or sets a list of tracks by this artist.
        /// </summary>
        public virtual List<TrackItem> Tracks { get; set; } = new List<TrackItem>();

        /// <summary>
        /// Gets or sets the last time this item was accessed.
        /// </summary>
        public DateTime LastAccessed { get; set; }
    }
}
