// <copyright file="PodcastShowItem.cs" company="Drastic Actions">
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
    /// Podcast Show Item.
    /// </summary>
    public class PodcastShowItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastShowItem"/> class.
        /// </summary>
        /// <param name="uri">URI for the podcast.</param>
        public PodcastShowItem(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            this.PodcastFeed = uri;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastShowItem"/> class.
        /// </summary>
        public PodcastShowItem()
        {
        }

        /// <summary>
        /// Gets or sets the Id of the Podcast.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the podcast.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the author of the podcast.
        /// </summary>
        public string? Author { get; set; }

        /// <summary>
        /// Gets or sets the description of the podcast.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the URI for the podcast.
        /// </summary>
        public Uri? PodcastFeed { get; set; }

        /// <summary>
        /// Gets or sets the Image URI for the podcast.
        /// </summary>
        public Uri? Image { get; set; }

        /// <summary>
        /// Gets or sets a list of episodes for this show.
        /// </summary>
        public virtual List<PodcastEpisodeItem>? Episodes { get; set; }
    }
}
