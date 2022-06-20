// <copyright file="PodcastLibrary.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Database;
using DrasticMedia.Core.Library;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Services;

namespace DrasticMedia.Podcast.Library
{
    /// <summary>
    /// Podcast Library.
    /// </summary>
    public class PodcastLibrary : MediaLibrary, IPodcastLibrary
    {
        private IPodcastDatabase podcastDatabase;
        private IPodcastService podcastService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastLibrary"/> class.
        /// </summary>
        /// <param name="service"><see cref="IPodcastService"/>.</param>
        /// <param name="database"><see cref="IPodcastDatabase"/>.</param>
        public PodcastLibrary(IPodcastService service, IPodcastDatabase database)
        {
            this.podcastDatabase = database;
            this.podcastService = service;
        }

        /// <inheritdoc/>
        public async Task<PodcastShowItem?> AddOrUpdatePodcastFromUri(Uri uri)
        {
            try
            {
                var podcast = await this.podcastDatabase.FetchShowViaUriAsync(uri).ConfigureAwait(false);
                var feed = await this.podcastService.FetchPodcastShowAsync(uri, CancellationToken.None).ConfigureAwait(false);
                if (feed == null)
                {
                    this.OnNewMediaItemError(new NewMediaItemErrorEventArgs() { MediaItemPath = uri.ToString(), MediaType = MediaType.Podcast });
                    return null;
                }

                if (podcast != null)
                {
                    await this.podcastDatabase.UpdatePodcastAsync(podcast);
                    this.OnUpdateMediaItemAdded(new UpdateMediaItemEventArgs(podcast));
                    return podcast;
                }

                await this.podcastDatabase.AddPodcastAsync(feed);
                this.OnNewMediaItemAdded(new NewMediaItemEventArgs(feed));
                return feed;
            }
            catch (Exception ex)
            {
                this.OnNewMediaItemError(new NewMediaItemErrorEventArgs() { Exception = ex, MediaItemPath = uri.ToString(), MediaType = MediaType.Podcast });
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<List<PodcastShowItem>> FetchPodcastsAsync() => await this.podcastDatabase.FetchShowsAsync().ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task<PodcastShowItem?> FetchPodcastWithEpisodesAsync(int showId) => await this.podcastDatabase.FetchShowWithEpisodesAsync(showId).ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task RemovePodcast(PodcastShowItem podcast)
        {
            podcast = await this.podcastDatabase.RemovePodcastAsync(podcast).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(podcast));
        }

        /// <inheritdoc/>
        public async Task RemovePodcastEpisode(PodcastEpisodeItem podcast)
        {
            podcast = await this.podcastDatabase.RemoveEpisodeAsync(podcast).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(podcast));
        }
    }
}
