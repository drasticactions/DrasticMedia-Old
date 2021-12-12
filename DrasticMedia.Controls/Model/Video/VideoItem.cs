// <copyright file="VideoItem.cs" company="Drastic Actions">
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
    /// Video Item.
    /// </summary>
    public class VideoItem : MediaItem
    {
        /// <summary>
        /// Gets or sets the Id of the item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the TV Show of the item.
        /// </summary>
        public TVShow? TvShow { get; set; }

        /// <summary>
        /// Gets or sets the TV Show Id of the item.
        /// </summary>
        public int? TvShowId { get; set; }

        /// <summary>
        /// Gets or sets the description of the video item.
        /// This should be a show episode description,
        /// or movie listing. Or... something else...
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the last time this item was accessed.
        /// </summary>
        public DateTime LastAccessed { get; set; }
    }
}
