// <copyright file="PodcastEpisodeListPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Events;
using DrasticMedia.Core.Utilities;
using DrasticMedia.ViewModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace DrasticMedia
{
    /// <summary>
    /// Podcast Episode List Page.
    /// </summary>
    public partial class PodcastEpisodeListPage : BasePage
    {
        private PodcastEpisodeListPageViewModel vm;
        private MediaWindow? window;

        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastEpisodeListPage"/> class.
        /// </summary>
        /// <param name="provider"><see cref="IServiceProvider"/>.</param>
        public PodcastEpisodeListPage(IServiceProvider provider, int podcastId)
            : base(provider)
        {
            this.InitializeComponent();
            this.ViewModel = this.vm = provider.ResolveWith<PodcastEpisodeListPageViewModel>(podcastId);
            this.BindingContext = this.vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.window = this.GetParentWindow() as MediaWindow;

            if (this.window != null)
            {
                this.window.SizeChanged += Window_SizeChanged;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (this.window != null)
            {
                this.window.SizeChanged -= Window_SizeChanged;
            }
        }

        private void Window_SizeChanged(object? sender, WindowOnSizeChangedEventArgs e)
        {
            if (e.Width <= 600)
            {
                VisualStateManager.GoToState(this.ImageHeader, "Small");
                VisualStateManager.GoToState(this.LabelGrid, "Small");
            }
            else
            {
                VisualStateManager.GoToState(this.ImageHeader, "Medium");
                VisualStateManager.GoToState(this.LabelGrid, "Medium");
            }
        }
    }
}
