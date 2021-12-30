// <copyright file="DesktopMusicArtistPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Utilities;
using DrasticMedia.Utilities;
using DrasticMedia.ViewModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace DrasticMedia
{
    public partial class DesktopMusicArtistPage : BasePage
    {
        private ArtistListPageViewModel artistVm;
        private AlbumListPageViewModel albumVm;

        public DesktopMusicArtistPage(IServiceProvider services)
            : base(services)
        {
            this.InitializeComponent();
            this.ViewModel = this.artistVm = services.ResolveWith<ArtistListPageViewModel>(this);
            this.albumVm = services.ResolveWith<AlbumListPageViewModel>(this);
            this.ArtistListLayout.BindingContext = this.artistVm;
            this.AlbumListLayout.BindingContext = this.albumVm;
            this.ArtistList.SelectionChanged += ArtistList_SelectionChanged;
        }

        private void ArtistList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Any() && e.CurrentSelection[0] is ArtistItem artist)
            {
                this.albumVm.LoadArtist(artist.Id).FireAndForgetSafeAsync();
            }
        }
    }
}
