// <copyright file="IVideoDatabase.cs" company="Drastic Actions">
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
    /// Video Database.
    /// </summary>
    public interface IVideoDatabase : IDatabase
    {
        /// <summary>
        /// Checks if the video exists in the database via the path.
        /// </summary>
        /// <param name="path">File path of the video.</param>
        /// <returns>Bool.</returns>
        Task<bool> ContainsVideoAsync(string path);

        /// <summary>
        /// Adds a video to the database.
        /// </summary>
        /// <param name="video">Video Item.</param>
        /// <returns>Updated VideoItem with Id set.</returns>
        Task<VideoItem> AddVideoItemAsync(VideoItem video);

        /// <summary>
        /// Removes a video from the database.
        /// </summary>
        /// <param name="video">Video Item.</param>
        /// <returns>VideoItem.</returns>
        Task<VideoItem> RemoveVideoItemAsync(VideoItem video);

        /// <summary>
        /// Adds a TV Show to the database.
        /// </summary>
        /// <param name="show">TVShow.</param>
        /// <returns>Updated TVShow with Id set.</returns>
        Task<TVShow> AddTVShowAsync(TVShow show);

        /// <summary>
        /// Removes a TV Show from the database.
        /// </summary>
        /// <param name="show">TVShow.</param>
        /// <returns>Removed TVShow.</returns>
        Task<TVShow> RemoveTVShowAsync(TVShow show);

        /// <summary>
        /// Fetches a TV Show via name.
        /// </summary>
        /// <param name="name">TVShow name.</param>
        /// <returns>TVShow.</returns>
        Task<TVShow> FetchTVShowViaNameAsync(string name);

        /// <summary>
        /// Fetches a TV Show with episodes.
        /// </summary>
        /// <param name="id">TVShow id.</param>
        /// <returns>TVShow.</returns>
        Task<TVShow> FetchTVShowWithEpisodesAsync(int id);

        /// <summary>
        /// Fetches all TV Shows.
        /// </summary>
        /// <returns>TVShows.</returns>
        Task<List<TVShow>> FetchTVShowsAsync();
    }
}
