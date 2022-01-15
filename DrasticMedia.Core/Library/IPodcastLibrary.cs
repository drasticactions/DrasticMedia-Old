// <copyright file="IPodcastLibrary.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Model;

namespace DrasticMedia.Core.Library
{
    public interface IPodcastLibrary : IMediaLibrary
    {
        /// <summary>
        /// Remove Podcast from Database.
        /// </summary>
        /// <param name="podcast">PodcastShowItem.</param>
        /// <returns>Task.</returns>
        Task RemovePodcast(PodcastShowItem podcast);

        /// <summary>
        /// Remove Podcast from Database.
        /// </summary>
        /// <param name="podcast">PodcastShowItem.</param>
        /// <returns>Task.</returns>
        Task RemovePodcastEpisode(PodcastEpisodeItem podcast);

        /// <summary>
        /// Fetch all podcasts.
        /// </summary>
        /// <returns>List of PodcastShowItem.</returns>
        Task<List<PodcastShowItem>> FetchPodcastsAsync();

        /// <summary>
        /// Fetch podcast with episodes.
        /// </summary>
        /// <param name="showId">Podcast show id.</param>
        /// <returns>PodcastShowItem.</returns>
        Task<PodcastShowItem?> FetchPodcastWithEpisodesAsync(int showId);

        /// <summary>
        /// Add or update a podcast via a uri.
        /// </summary>
        /// <param name="uri">Uri of podcast.</param>
        /// <returns>Podcast Show Item.</returns>
        Task<PodcastShowItem?> AddOrUpdatePodcastFromUri(Uri uri);
    }
}
