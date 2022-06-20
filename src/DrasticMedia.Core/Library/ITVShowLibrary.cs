// <copyright file="ITVShowLibrary.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Model;

namespace DrasticMedia.Core.Library
{
    /// <summary>
    /// TVShow Library.
    /// </summary>
    public interface ITVShowLibrary : IMediaLibrary
    {
        /// <summary>
        /// Fetches all TV Shows.
        /// </summary>
        /// <returns>TVShows.</returns>
        Task<List<TVShow>> FetchTVShowsAsync();

        /// <summary>
        /// Fetches a TV Show with episodes.
        /// </summary>
        /// <param name="id">TVShow id.</param>
        /// <returns>TVShow.</returns>
        Task<TVShow?> FetchTVShowWithEpisodesAsync(int id);

        /// <summary>
        /// Fetches a TV Show via name.
        /// </summary>
        /// <param name="name">TVShow name.</param>
        /// <returns>TVShow.</returns>
        Task<TVShow?> FetchTVShowViaNameAsync(string name);

        /// <summary>
        /// Remove TVShow from database.
        /// </summary>
        /// <param name="show">TVShow to remove.</param>
        /// <returns>Task.</returns>
        Task RemoveTVShowAsync(TVShow show);
    }
}
