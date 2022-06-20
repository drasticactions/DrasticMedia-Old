// <copyright file="PodcastService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

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
        private readonly HttpClient httpClient;
        private static readonly XmlSerializer XmlSerializer = new(typeof(Rss));
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastService"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public PodcastService(ILogger logger)
        {
            this.logger = logger;
            this.httpClient = new HttpClient();
        }

        /// <inheritdoc/>
        public async Task<PodcastShowItem?> FetchPodcastShowAsync(Uri podcastUri, CancellationToken cancellationToken)
        {
            try
            {
                using var feedContent = await this.httpClient.GetStreamAsync(podcastUri);
                var rss = XmlSerializer.Deserialize(feedContent) as Rss;
                if (rss is null)
                {
                    throw new ArgumentNullException("Feed not RSS");
                }

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
