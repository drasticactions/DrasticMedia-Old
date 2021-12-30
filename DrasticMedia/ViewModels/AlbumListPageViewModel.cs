// <copyright file="AlbumListPageViewModel.cs" company="Drastic Actions">
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
using DrasticMedia.Core.Services;
using DrasticMedia.Core.Utilities;
using DrasticMedia.Utilities;

namespace DrasticMedia.ViewModels
{
    public class AlbumListPageViewModel : BaseViewModel
    {
        private int artistId;
        private ArtistItem? artist;
        private AsyncCommand<AlbumItem>? playAlbumItemCommand;

        public AlbumListPageViewModel(IServiceProvider services, Page originalPage = null, int artistId = 0)
            : base(services, originalPage)
        {
            this.artistId = artistId;
        }

        /// <summary>
        /// Gets the podcast.
        /// </summary>
        public ArtistItem? Artist => this.artist;

        /// <summary>
        /// Gets the add navigate to podcast command.
        /// </summary>
        public AsyncCommand<AlbumItem> PlayAlbumItemCommand
        {
            get
            {
                return this.playAlbumItemCommand ??= new AsyncCommand<AlbumItem>(this.PlayAlbumItem, null, this.Error);
            }
        }

        /// <summary>
        /// Gets the list of episodes.
        /// </summary>
        public List<AlbumItem>? Albums => this.artist?.Albums ?? new List<AlbumItem>();

        /// <inheritdoc/>
        public override async Task LoadAsync()
        {
            await base.LoadAsync();
            if (this.artist == null && this.artistId > 0)
            {
                await this.LoadArtist(this.artistId);
            }
        }

        /// <summary>
        /// Load Artist.
        /// </summary>
        /// <param name="artistId">Artist Id.</param>
        /// <returns>Task.</returns>
        public async Task LoadArtist(int artistId)
        {
            this.artistId = artistId;
            this.artist = await this.MediaLibrary.FetchArtistWithAlbumsViaIdAsync(artistId);
            this.OnPropertyChanged(nameof(this.Artist));
            this.OnPropertyChanged(nameof(this.Albums));
        }

        private async Task PlayAlbumItem(AlbumItem item)
        {
            var newPage = this.Services.ResolveWith<AlbumPage>(item.Id);
            await this.Navigation.PushPageInWindowViaPageAsync(newPage, this.OriginalPage);
        }
    }
}
