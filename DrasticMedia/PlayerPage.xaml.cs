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
