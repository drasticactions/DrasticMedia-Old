// <copyright file="IVideoLibrary.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Model;

namespace DrasticMedia.Core.Library
{
    /// <summary>
    /// Video Library.
    /// </summary>
    public interface IVideoLibrary : IMediaLibrary
    {
        /// <summary>
        /// Fetches all VideoItems.
        /// </summary>
        /// <returns>VideoItem.</returns>
        Task<List<VideoItem>> FetchVideosAsync();
    }
}
