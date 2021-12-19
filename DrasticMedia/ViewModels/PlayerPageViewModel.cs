// <copyright file="PlayerPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Services;

namespace DrasticMedia.ViewModels
{
    /// <summary>
    /// Player Page View Model.
    /// </summary>
    public class PlayerPageViewModel : BaseViewModel
    {
        private PlayerService player;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPageViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public PlayerPageViewModel(IServiceProvider services)
            : base(services)
        {
            this.player = services.GetService<PlayerService>();
            if (this.player == null)
            {
                throw new ArgumentNullException(nameof(this.player));
            }
        }

        /// <summary>
        /// Gets the current player service.
        /// </summary>
        public PlayerService Player => this.player;
    }
}
