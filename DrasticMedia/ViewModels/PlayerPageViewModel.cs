// <copyright file="PlayerPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Services;
using DrasticMedia.Utilities;

namespace DrasticMedia.ViewModels
{
    /// <summary>
    /// Player Page View Model.
    /// </summary>
    public class PlayerPageViewModel : BaseViewModel
    {
        private PlayerService player;
        private PlayerPage playerPage;
        private AsyncCommand? hidePlayerPageCommand;
        private AsyncCommand? showPlayerPageCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPageViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        /// <param name="playerPage">Player Page.</param>
        public PlayerPageViewModel(IServiceProvider services, PlayerPage playerPage)
            : base(services)
        {
            this.player = services.GetService<PlayerService>();
            this.playerPage = playerPage;

            if (this.player == null)
            {
                throw new ArgumentNullException(nameof(this.player));
            }

            if (this.playerPage == null)
            {
                throw new ArgumentNullException(nameof(this.playerPage));
            }
        }

        /// <summary>
        /// Gets the hide player page command.
        /// </summary>
        public AsyncCommand ShowPlayerPageCommand
        {
            get
            {
                return this.showPlayerPageCommand ??= new AsyncCommand(async () => this.SetPlayerPageVisibility(0), null, this.Error);
            }
        }

        /// <summary>
        /// Gets the hide player page command.
        /// </summary>
        public AsyncCommand HidePlayerPageCommand
        {
            get
            {
                return this.hidePlayerPageCommand ??= new AsyncCommand(async () => this.SetPlayerPageVisibility(2000), null, this.Error);
            }
        }

        /// <summary>
        /// Gets the current player service.
        /// </summary>
        public PlayerService Player => this.player;

        private void SetPlayerPageVisibility(double y)
        {
            this.playerPage.SetPlayerVisiblity(y);
        }
    }
}
