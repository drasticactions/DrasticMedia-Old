// <copyright file="ArtistListPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Library;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Utilities;
using DrasticMedia.Utilities;

namespace DrasticMedia.ViewModels
{
    public class ArtistListPageViewModel : BaseViewModel
    {
        public ArtistListPageViewModel(IServiceProvider services, Page originalPage = null)
            : base(services, originalPage)
        {
        }

        /// <summary>
        /// Gets the list of artists.
        /// </summary>
        public ObservableCollection<ArtistItem> Artists { get; private set; } = new ObservableCollection<ArtistItem>();

        /// <inheritdoc/>
        public override async Task LoadAsync()
        {
            await base.LoadAsync();
            if (this.Artists.Any())
            {
                return;
            }

            await RefreshArtists();
        }

        private async Task RefreshArtists()
        {
            this.Artists.Clear();
            var artists = await this.MediaLibrary.FetchArtistsAsync();
            foreach (var artist in artists)
            {
                this.Artists.Add(artist);
            }
        }
    }
}
