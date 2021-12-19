// <copyright file="PodcastListPageViewModel.cs" company="Drastic Actions">
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
using DrasticMedia.Core.Utilities;
using DrasticMedia.Utilities;

namespace DrasticMedia.ViewModels
{
    /// <summary>
    /// Podcast Page View Model.
    /// </summary>
    public class PodcastListPageViewModel : BaseViewModel
    {
        private AsyncCommand? addNewPodcastFeedItemCommand;

        private AsyncCommand<PodcastShowItem>? navigateToPodcastCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastListPageViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        /// <param name="page">Page.</param>
        public PodcastListPageViewModel(IServiceProvider services, Page page)
            : base(services, page)
        {
        }

        /// <summary>
        /// Gets the add new podcast feed command.
        /// </summary>
        public AsyncCommand AddNewPodcastFeedItemCommand
        {
            get
            {
                return this.addNewPodcastFeedItemCommand ??= new AsyncCommand(this.AddNewFeedListItemAsync, null, this.Error);
            }
        }

        /// <summary>
        /// Gets the add navigate to podcast command.
        /// </summary>
        public AsyncCommand<PodcastShowItem> NavigateToPodcastCommand
        {
            get
            {
                return this.navigateToPodcastCommand ??= new AsyncCommand<PodcastShowItem>(this.NavigateToPodcastShow, null, this.Error);
            }
        }


        /// <summary>
        /// Gets the list of shows.
        /// </summary>
        public ObservableCollection<PodcastShowItem> Shows { get; private set; } = new ObservableCollection<PodcastShowItem>();

        /// <summary>
        /// Add new feed list item.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task AddNewFeedListItemAsync()
        {
            var feedUri = await this.Navigation.DisplayPromptInWindowViaPageAsync(this.CheckIfPageExists(), Translations.Common.AddNewPodcastFeedTitle, Translations.Common.AddNewPodcastFeedMessage);
            if (string.IsNullOrEmpty(feedUri) || !new Uri(feedUri).IsWellFormedOriginalString())
            {
                return;
            }

            var podcastShowItem = await this.MediaLibrary.AddOrUpdatePodcastFromUri (new Uri(feedUri));
            if (podcastShowItem != null && !this.Shows.Contains(podcastShowItem))
            {
                this.Shows.Add(podcastShowItem);
            }
        }

        /// <inheritdoc/>
        public override async Task LoadAsync()
        {
            await base.LoadAsync();
            if (this.Shows.Any())
            {
                return;
            }

            await RefreshFeed();
        }

        private async Task NavigateToPodcastShow(PodcastShowItem item)
        {
            if (item == null)
            {
                return;
            }

            var podcastEpisodePage = Services.ResolveWith<PodcastEpisodeListPage>(item.Id);
            await this.Navigation.PushPageInWindowViaPageAsync(podcastEpisodePage, this.CheckIfPageExists());
        }

        private async Task RefreshFeed()
        {
            this.Shows.Clear();
            var podcasts = await this.MediaLibrary.FetchPodcastsAsync();
            foreach (var podcast in podcasts)
            {
                this.Shows.Add(podcast);
            }
        }
    }
}
