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
        PlayerService player;

        public PlayerPage(IServiceProvider provider)
        {
            this.InitializeComponent();
            this.player = provider.GetService<PlayerService>();

            //HACK: For some reason, the binding isn't working and I need to get the propery off of the service???
            this.player.PropertyChanged += Player_PropertyChanged;
            this.BindingContext = this.player;
            this.DrasticSlider.NewPositionRequested += DrasticSlider_NewPositionRequested;
            this.TestButton.Clicked += TestButton_Clicked;
        }

        private void Player_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PlayerService.CurrentPosition))
            {
                this.DrasticSlider.Value = this.player.CurrentPosition;
            }
        }

        private void TestButton_Clicked(object sender, EventArgs e)
        {
            var song = new TrackItem() { Path = @"" };
            this.player.AddMedia(song, true);
        }

        private void DrasticSlider_NewPositionRequested(object sender, Core.DrasticSliderPositionChangedEventArgs e)
        {
            if (this.player?.CurrentPosition != null)
            {
                this.player.CurrentPosition = e.Position;
            }
        }

        //private async void PlayerPage_Drop(object sender, Overlays.DragAndDropOverlayTappedEventArgs e)
        //{
        //    var song = new TrackItem() { Path = e.Path };
        //    await this.player.AddMedia(song, true);
        //    var artwork = await this.player.MediaService.GetArtworkUrl();
        //    var imageSource = ImageSource.FromFile(artwork);
        //    this.Dispatcher.Dispatch(() => this.TestAlbumArt.Source = imageSource);
        //}

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    ((MediaWindow)this.GetParentWindow()).Drop += PlayerPage_Drop;
        //}
    }
}
