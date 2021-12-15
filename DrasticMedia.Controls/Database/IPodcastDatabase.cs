// <copyright file="IPodcastDatabase.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;

namespace DrasticMedia.Core.Database
{
    /// <summary>
    /// Podcast Database.
    /// </summary>
    public interface IPodcastDatabase : IDatabase
    {
        /// <summary>
        /// Gets an list of <see cref="PodcastShowItem"/>.
        /// </summary>
        /// <returns>List of PodcastShowItem.</returns>
        Task<List<PodcastShowItem>> FetchShowsAsync();

        /// <summary>
        /// Gets a <see cref="PodcastShowItem"/>.
        /// </summary>
        /// <param name="showId">Podcast Show Id.</param>
        /// <returns> PodcastShowItem.</returns>
        Task<PodcastShowItem> FetchShowWithEpisodesAsync(int showId);

        /// <summary>
        /// Gets a <see cref="PodcastShowItem"/>.
        /// </summary>
        /// <param name="showId">Podcast Show Id.</param>
        /// <returns> PodcastShowItem.</returns>
        Task<PodcastShowItem> FetchShowAsync(int showId);

        /// <summary>
        /// Gets an list of <see cref="PodcastEpisodeItem"/>.
        /// </summary>
        /// <returns>List of PodcastEpisodeItem.</returns>
        Task<List<PodcastEpisodeItem>> FetchAllEpisodesAsync();

        /// <summary>
        /// Gets an list of <see cref="PodcastEpisodeItem"/>.
        /// </summary>
        /// <param name="showId">Podcast Show Id.</param>
        /// <returns>List of PodcastEpisodeItem.</returns>
        Task<List<PodcastEpisodeItem>> FetchEpisodesAsync(int showId);

        /// <summary>
        /// Gets an <see cref="PodcastEpisodeItem"/>.
        /// </summary>
        /// <param name="episodeId">Podcast Id.</param>
        /// <returns>PodcastEpisodeItem.</returns>
        Task<PodcastEpisodeItem> FetchEpisodeAsync(int episodeId);

        /// <summary>
        /// Adds an podcast to the database.
        /// </summary>
        /// <param name="podcast">PodcastShowItem.</param>
        /// <returns>Updated PodcastShowItem with Id set.</returns>
        Task<PodcastShowItem> AddPodcastAsync(PodcastShowItem podcast);

        /// <summary>
        /// Adds an podcast episode to the database.
        /// </summary>
        /// <param name="episode">PodcastEpisodeItem.</param>
        /// <returns>Updated PodcastEpisodeItem with Id set.</returns>
        Task<PodcastEpisodeItem> AddEpisodeAsync(PodcastEpisodeItem episode);

        /// <summary>
        /// Adds an podcast episode to the database.
        /// </summary>
        /// <param name="episodes">List of PodcastEpisodeItem.</param>
        /// <returns>Updated PodcastEpisodeItem with Id set.</returns>
        Task<List<PodcastEpisodeItem>> AddEpisodesAsync(List<PodcastEpisodeItem> episodes);

        /// <summary>
        /// Updates an podcast to the database.
        /// </summary>
        /// <param name="podcast">PodcastShowItem.</param>
        /// <returns>Updated PodcastShowItem with Id set.</returns>
        Task<PodcastShowItem> UpdatePodcastAsync(PodcastShowItem podcast);

        /// <summary>
        /// Updates an podcast episode to the database.
        /// </summary>
        /// <param name="episode">PodcastEpisodeItem.</param>
        /// <returns>Updated PodcastEpisodeItem with Id set.</returns>
        Task<PodcastEpisodeItem> UpdateEpisodeAsync(PodcastEpisodeItem episode);

        /// <summary>
        /// Remove PodcastShowItem from database.
        /// </summary>
        /// <param name="show">PodcastShowItem.</param>
        /// <returns>Removed Item.</returns>
        Task<PodcastShowItem> RemovePodcastAsync(PodcastShowItem show);

        /// <summary>
        /// Remove PodcastEpisodeItem from database.
        /// </summary>
        /// <param name="episode">PodcastEpisodeItem.</param>
        /// <returns>Removed Item.</returns>
        Task<PodcastEpisodeItem> RemoveEpisodeAsync(PodcastEpisodeItem episode);
    }
}
