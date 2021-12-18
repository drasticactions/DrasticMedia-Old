// <copyright file="PodcastListPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastListPage"/> class.
        /// </summary>
        /// <param name="provider"><see cref="IServiceProvider"/>.</param>
        public PodcastListPage(IServiceProvider provider)
            : base(provider)
        {
            this.InitializeComponent();
        }
    }
}
