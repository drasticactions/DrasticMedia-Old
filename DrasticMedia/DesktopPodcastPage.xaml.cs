// <copyright file="DesktopPodcastPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Utilities;
using DrasticMedia.Utilities;
using DrasticMedia.ViewModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace DrasticMedia
{
    public partial class DesktopPodcastPage : BasePage
    {
        private PodcastListPageViewModel podcastListViewModel;
        private PodcastEpisodeListPageViewModel podcastEpisodeViewModel;

        public DesktopPodcastPage(IServiceProvider services)
            : base(services)
        {
            this.InitializeComponent();
            this.ViewModel = this.podcastListViewModel = services.ResolveWith<PodcastListPageViewModel>(this);
            this.podcastEpisodeViewModel = services.ResolveWith<PodcastEpisodeListPageViewModel>(this, 0);
            this.PodcastEpisodeListLayout.BindingContext = this.podcastEpisodeViewModel;
            this.PodcastListLayout.BindingContext = this.ViewModel;
            this.PodcastShowList.SelectionChanged += this.PodcastShowList_SelectionChanged;
        }

        private void PodcastShowList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Any() && e.CurrentSelection.First() is PodcastShowItem show)
            {
                this.podcastEpisodeViewModel.LoadPodcast(show.Id).FireAndForgetSafeAsync();
            }
        }
    }
}
