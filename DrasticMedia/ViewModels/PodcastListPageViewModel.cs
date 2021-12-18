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
using DrasticMedia.Utilities;

namespace DrasticMedia.ViewModels
{
    /// <summary>
    /// Podcast Page View Model.
    /// </summary>
    public class PodcastListPageViewModel : BaseViewModel
    {
        private MediaLibrary library;
        private AsyncCommand? addNewPodcastFeedItemCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastListPageViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public PodcastListPageViewModel(IServiceProvider services)
            : base(services)
        {
            this.library = services.GetService<MediaLibrary>();
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

        public ObservableCollection<PodcastShowItem> Shows { get; private set; } = new ObservableCollection<PodcastShowItem>();

        /// <summary>
        /// Add new feed list item.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task AddNewFeedListItemAsync()
        {
            var feedUri = await this.Navigation.DisplayPromptAsync(Translations.Common.AddNewPodcastFeedTitle, Translations.Common.AddNewPodcastFeedMessage);
            if (string.IsNullOrEmpty(feedUri) || !new Uri(feedUri).IsWellFormedOriginalString())
            {
                return;
            }

            var podcastShowItem = await this.library.AddOrUpdatePodcastFromUri (new Uri(feedUri));
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
