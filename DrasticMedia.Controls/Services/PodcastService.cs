// <copyright file="PodcastService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using DrasticMedia.Core.Infrastructure;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Model.Feeds;

namespace DrasticMedia.Core.Services
{
    /// <summary>
    /// Podcast Service.
    /// </summary>
    public class PodcastService : IPodcastService
    {
        private static readonly XmlSerializer XmlSerializer = new(typeof(Rss));
        private ILogger logger;
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastService"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public PodcastService (ILogger logger)
        {
            this.logger = logger;
            this.httpClient = new HttpClient ();
        }

        /// <inheritdoc/>
        public async Task<PodcastShowItem?> FetchPodcastShowAsync(Uri podcastUri, CancellationToken cancellationToken)
        {
            try
            {
                await using var feedContent = await this.httpClient.GetStreamAsync(podcastUri, cancellationToken);
                var rss = (Rss)XmlSerializer.Deserialize(feedContent);
                var updatedShow = Mapper.Map(podcastUri, rss);
                return updatedShow;
            }
            catch (Exception ex)
            {
                this.logger.Log(ex);
                throw;
            }
        }
    }
}
