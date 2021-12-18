// <copyright file="PodcastEpisodeListPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;

namespace DrasticMedia.ViewModels
{
    /// <summary>
    /// Podcast Episode List Page View Model.
    /// </summary>
    public class PodcastEpisodeListPageViewModel : BaseViewModel
    {
        private PodcastShowItem? show;
        private int podcastId;

        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastEpisodeListPageViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        /// <param name="podcastId">Podcast Id.</param>
        public PodcastEpisodeListPageViewModel(IServiceProvider services, int podcastId)
            : base(services)
        {
            this.podcastId = podcastId;
        }

        /// <summary>
        /// Gets the podcast.
        /// </summary>
        public PodcastShowItem? Show => this.show;

        /// <summary>
        /// Gets the list of episodes.
        /// </summary>
        public List<PodcastEpisodeItem>? Episodes => this.show?.Episodes ?? new List<PodcastEpisodeItem>();

        /// <inheritdoc/>
        public override async Task LoadAsync()
        {
            await base.LoadAsync();
            if (this.show == null)
            {
                await this.LoadPodcast(this.podcastId);
            }
        }

        private async Task LoadPodcast(int podcastId)
        {
            this.show = await this.MediaLibrary.FetchPodcastWithEpisodesAsync(podcastId);
            this.OnPropertyChanged(nameof(this.Show));
            this.OnPropertyChanged(nameof(this.Episodes));
        }
    }
}
