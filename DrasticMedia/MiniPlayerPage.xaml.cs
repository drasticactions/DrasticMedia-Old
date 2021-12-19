// <copyright file="MiniPlayerPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Services;
using DrasticMedia.Core.Tools;
using DrasticMedia.ViewModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace DrasticMedia
{
    /// <summary>
    /// Mini Player Page.
    /// </summary>
    public partial class MiniPlayerPage : BasePage, IHitTestPage
    {
        private PlayerPageViewModel? vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="MiniPlayerPage"/> class.
        /// </summary>
        /// <param name="provider"><see cref="IServiceProvider"/>.</param>
        public MiniPlayerPage(IServiceProvider provider)
            : base(provider)
        {
            this.InitializeComponent();
            this.HitTestViews = new List<IView>() { this.PlayerControls };
            this.ViewModel = this.vm = provider.GetService<PlayerPageViewModel>();
            this.BindingContext = this.ViewModel;
            this.vm.Player.IsPlayingChanged += this.Player_IsPlayingChanged;
        }

        /// <summary>
        /// Gets the hit test views.
        /// </summary>
        public List<IView> HitTestViews { get; }

        private void Player_IsPlayingChanged(object? sender, EventArgs e)
        {
            this.SetPlayerVisibility();
        }

        private void SetPlayerVisibility()
        {
            if (!this.vm.Player.HasCurrentMediaSet)
            {
                this.PlayerControls.TranslateTo(0, 50);
            }
            else
            {
                this.PlayerControls.TranslateTo(0, 0);
            }
        }
    }
}
