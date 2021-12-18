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
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace DrasticMedia
{
    public partial class PlayerPage : ContentPage
    {
        private PlayerService player;
        private WindowTappedService windowTappedService;

        public PlayerPage(IServiceProvider provider)
        {
            this.InitializeComponent();
            this.player = provider.GetService<PlayerService>();

            //HACK: For some reason, the binding isn't working and I need to get the propery off of the service???
            this.player.PropertyChanged += Player_PropertyChanged;
            this.BindingContext = this.player;
            this.DrasticSlider.NewPositionRequested += this.DrasticSlider_NewPositionRequested;
        }

        private void Player_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PlayerService.CurrentPosition))
            {
                this.DrasticSlider.Value = this.player.CurrentPosition;
            }
        }

        private void DrasticSlider_NewPositionRequested(object sender, Core.DrasticSliderPositionChangedEventArgs e)
        {
            if (this.player?.CurrentPosition != null)
            {
                this.player.CurrentPosition = e.Position;
            }
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    if (this.player != null)
        //    {
        //        this.windowTappedService = new WindowTappedService(this.GetParentWindow(), this.player.MediaService);
        //    }

        //    this.windowTappedService.OnHidden += WindowTappedService_OnHidden;
        //    this.windowTappedService.OnTapped += WindowTappedService_OnTapped;
        //}

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();

        //    if (this.windowTappedService != null)
        //    {
        //        this.windowTappedService.OnHidden -= WindowTappedService_OnHidden;
        //        this.windowTappedService.OnTapped -= WindowTappedService_OnTapped;
        //    }
        //}

        private async void WindowTappedService_OnTapped(object sender, EventArgs e)
        {
            await this.PlayerControls.FadeTo(1, 250, Easing.CubicIn);
            //this.Dispatcher.Dispatch(() => this.PlayerControls.IsVisible = true);
        }

        private async void WindowTappedService_OnHidden(object sender, EventArgs e)
        {
            await this.PlayerControls.FadeTo(0, 250, Easing.CubicOut);
            //this.Dispatcher.Dispatch(() => this.PlayerControls.IsVisible = false);
        }
    }
}
