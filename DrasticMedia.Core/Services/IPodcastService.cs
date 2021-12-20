// <copyright file="IPodcastService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;

namespace DrasticMedia.Core.Services
{
    /// <summary>
    /// Podcast Service.
    /// </summary>
    public interface IPodcastService
    {
        /// <summary>
        /// Fetch a podcast from a RSS feed URI.
        /// </summary>
        /// <param name="podcastUri">Podcast URI.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns><see cref="PodcastShowItem"/>.</returns>
        Task<PodcastShowItem?> FetchPodcastShowAsync(Uri podcastUri, CancellationToken cancellationToken);
    }
}
