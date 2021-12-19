// <copyright file="PlayerPage.xaml.cs" company="Drastic Actions">
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
    /// Player Page.
    /// </summary>
    public partial class PlayerPage : BasePage, IHitTestPage
    {
        private PlayerPageViewModel vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPage"/> class.
        /// </summary>
        /// <param name="provider"><see cref="IServiceProvider"/>.</param>
        public PlayerPage(IServiceProvider provider)
            : base(provider)
        {
            this.InitializeComponent();
            this.HitTestViews = new List<IView>() { this.PlayerControls };
            this.ViewModel = this.vm = provider.GetService<PlayerPageViewModel>();
            this.BindingContext = this.ViewModel;

            //HACK: For some reason, the binding isn't working and I need to get the property off of the service???
            this.vm.Player.PropertyChanged += Player_PropertyChanged;
            this.DrasticSlider.NewPositionRequested += this.DrasticSlider_NewPositionRequested;
        }

        public List<IView> HitTestViews { get; }

        private void Player_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PlayerService.CurrentPosition))
            {
                this.DrasticSlider.Value = this.vm.Player.CurrentPosition;
            }
        }

        private void DrasticSlider_NewPositionRequested(object sender, Core.DrasticSliderPositionChangedEventArgs e)
        {
            if (this.vm.Player?.CurrentPosition != null)
            {
                this.vm.Player.CurrentPosition = e.Position;
            }
        }
    }
}
