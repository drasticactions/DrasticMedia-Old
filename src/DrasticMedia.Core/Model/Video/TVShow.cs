// <copyright file="TVShow.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DrasticMedia.Core.Model
{
    /// <summary>
    /// TV Show.
    /// </summary>
    public class TVShow
    {
        /// <summary>
        /// Gets or sets the Id of the item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the show title.
        /// </summary>
        public string ShowTitle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the poster path.
        /// </summary>
        public string PosterPath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the episodes of the show.
        /// </summary>
        public virtual List<VideoItem> Episodes { get; set; } = new List<VideoItem>();
    }
}
