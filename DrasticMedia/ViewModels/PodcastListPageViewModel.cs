// <copyright file="PodcastListPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticMedia.ViewModels
{
    /// <summary>
    /// Podcast Page View Model.
    /// </summary>
    public class PodcastListPageViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastListPageViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public PodcastListPageViewModel(IServiceProvider services)
            : base(services)
        {
        }
    }
}
