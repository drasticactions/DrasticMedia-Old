// <copyright file="PodcastListPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Utilities;
using DrasticMedia.Overlays;
using DrasticMedia.ViewModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace DrasticMedia
{
    /// <summary>
    /// Podcast List Page.
    /// </summary>
    public partial class PodcastListPage : BasePage
    {
        private PodcastListPageViewModel? vm;
        private MediaWindow? window;
        private int currentGridSize = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastListPage"/> class.
        /// </summary>
        /// <param name="provider"><see cref="IServiceProvider"/>.</param>
        public PodcastListPage(IServiceProvider provider)
            : base(provider)
        {
            this.InitializeComponent();
            this.ViewModel = this.vm = provider.ResolveWith<PodcastListPageViewModel>(this);
            this.BindingContext = this.vm;
            //this.GridItemlayoutSettings.Span = this.currentGridSize;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (this.window == null)
            {
                this.window = this.GetParentWindow() as MediaWindow;
                if (this.window == null)
                {
                    return;
                }

                this.window.SizeChanged += this.Overlay_SizeChanged;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (this.window != null)
            {
                this.window.SizeChanged -= this.Overlay_SizeChanged;
            }
        }

        private void Overlay_SizeChanged(object? sender, EventArgs e)
        {
            //var max = (int)this.PodcastGrid.Width / 200;
            //if (max != this.currentGridSize)
            //{
            //    this.currentGridSize = max;
            //    this.Dispatcher.Dispatch(() => {

            //        this.GridItemlayoutSettings.Span = this.currentGridSize;
            //    });
            //}
        }
    }
}
