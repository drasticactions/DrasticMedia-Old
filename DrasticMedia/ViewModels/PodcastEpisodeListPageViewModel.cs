// <copyright file="PodcastEpisodeListPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Library;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Services;
using DrasticMedia.Core.Utilities;
using DrasticMedia.Utilities;

namespace DrasticMedia.ViewModels
{
    /// <summary>
    /// Podcast Episode List Page View Model.
    /// </summary>
    public class PodcastEpisodeListPageViewModel : BaseViewModel
    {
        private PodcastShowItem? show;
        private int podcastId;
        private AsyncCommand<PodcastEpisodeItem>? playPodcastEpisodeCommand;
        private PlayerService? playerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastEpisodeListPageViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        /// <param name="page">Page.</param>
        /// <param name="podcastId">Podcast Id.</param>
        public PodcastEpisodeListPageViewModel(IServiceProvider services, Page page, int podcastId)
            : base(services, page)
        {
            this.podcastId = podcastId;
            this.playerService = services.GetService<PlayerService>();
            if (this.playerService == null)
            {
                throw new ArgumentNullException(nameof(this.playerService));
            }
        }

        /// <summary>
        /// Gets the add navigate to podcast command.
        /// </summary>
        public AsyncCommand<PodcastEpisodeItem> PlayPodcastEpisodeCommand
        {
            get
            {
                return this.playPodcastEpisodeCommand ??= new AsyncCommand<PodcastEpisodeItem>(this.PlayPodcastEpisode, null, this.Error);
            }
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

        private async Task PlayPodcastEpisode(PodcastEpisodeItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await this.playerService.AddMedia(item, true);
        }

        private async Task LoadPodcast(int podcastId)
        {
            this.show = await this.MediaLibrary.FetchPodcastWithEpisodesAsync(podcastId);
            this.OnPropertyChanged(nameof(this.Show));
            this.OnPropertyChanged(nameof(this.Episodes));
        }
    }
}