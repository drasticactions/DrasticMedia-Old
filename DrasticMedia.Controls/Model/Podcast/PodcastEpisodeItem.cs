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
        /// Initializes a new instance of the <see cref="PodcastEpisodeItem"/> class.
        /// </summary>
        public PodcastEpisodeItem()
        {
        }

        public PodcastEpisodeItem(string description, TimeSpan? duration, bool @explicit, DateTime published, string title, string url, string albumArtUri)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            this.Description = description;

            if (duration != null)
            {
                this.Duration = duration.Value;
            }

            this.Explicit = @explicit;
            this.ReleaseDate = published;
            this.Title = title;
            this.OnlinePath = new Uri(url);
            this.AlbumArtUri = new Uri(albumArtUri);
        }

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
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the episode is downloaded locally.
        /// If it is, use <see cref="MediaItem.Path"/> to get the local download.
        /// </summary>
        public bool IsDownloaded { get; set; }

        /// <summary>
        /// Gets or sets the description of the podcast Episode.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the show is explicit.
        /// </summary>
        public bool Explicit { get; set; }

        /// <summary>
        /// Gets or sets the Podcast Show for the episode.
        /// </summary>
        public virtual PodcastShowItem? PodcastShowItem { get; set; }
    }

    /// <summary>
    /// Podcast Episode Item Extensions.
    /// </summary>
    public static class PodcastEpisodeItemExtensions
    {
        /// <summary>
        /// Updates an existing podcast episode.
        /// </summary>
        /// <param name="item">The original podcast episode.</param>
        /// <param name="update">The updated podcast episode.</param>
        public static void UpdateEpisode(this PodcastEpisodeItem item, PodcastEpisodeItem update)
        {
            item.Title = update.Title;
            item.PodcastShowId = update.PodcastShowId;
            item.ReleaseDate = update.ReleaseDate;
            item.Description = update.Description;
            item.OnlinePath = update.OnlinePath;
            item.Explicit = update.Explicit;
            item.AlbumArtUri = update.AlbumArtUri;
        }
    }
}
