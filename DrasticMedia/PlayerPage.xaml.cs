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
using DrasticMedia.ViewModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace DrasticMedia
{
    public partial class PlayerPage : ContentPage
    {
        private PlayerPageViewModel vm;

        public PlayerPage(IServiceProvider provider)
        {
            this.InitializeComponent();
            this.vm = provider.GetService<PlayerPageViewModel>();
            this.BindingContext = this.vm;

            //HACK: For some reason, the binding isn't working and I need to get the property off of the service???
            this.vm.Player.PropertyChanged += Player_PropertyChanged;
            this.DrasticSlider.NewPositionRequested += this.DrasticSlider_NewPositionRequested;
        }

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
