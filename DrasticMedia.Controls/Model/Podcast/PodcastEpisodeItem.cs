// <copyright file="PodcastEpisodeItem.cs" company="Drastic Actions">
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
    /// Podcast Episode Item.
    /// </summary>
    public class PodcastEpisodeItem : MediaItem
    {
        /// <summary>
        /// Gets or sets the Id of the Podcast Episode.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the PodcastShowId of the episode.
        /// </summary>
        public int PodcastShowId { get; set; }

        /// <summary>
        /// Gets or sets the release date of the episode.
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// Gets or sets the description of the podcast Episode.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the Podcast Show for the episode.
        /// </summary>
        public virtual PodcastShowItem? PodcastShowItem { get; set; }
    }
}
