// <copyright file="AlbumListPageViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core;
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
            this.MediaLibrary.NewMediaItemAdded += this.Library_NewMediaItemAdded;
            this.MediaLibrary.NewMediaItemError += this.Library_NewMediaItemError;
            this.MediaLibrary.RemoveMediaItem += this.Library_RemoveMediaItem;
            this.MediaLibrary.UpdateMediaItemAdded += this.Library_UpdateMediaItemAdded;
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
        public ObservableCollection<AlbumItem> Albums { get; private set; } = new ObservableCollection<AlbumItem>();

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
            this.Albums.Clear();

            foreach (var album in this.artist.Albums)
            {
                this.Albums.Add(album);
            }

            this.OnPropertyChanged(nameof(this.Artist));
            this.OnPropertyChanged(nameof(this.Albums));
        }

        private async Task PlayAlbumItem(AlbumItem? item)
        {
            if (item is null)
            {
                return;
            }

            var newPage = this.Services.ResolveWith<AlbumPage>(item.Id);
            await this.Navigation.PushPageInWindowViaPageAsync(newPage, this.OriginalPage);
        }

        private void Library_UpdateMediaItemAdded(object sender, UpdateMediaItemEventArgs e)
        {
        }

        private void Library_RemoveMediaItem(object sender, RemoveMediaItemEventArgs e)
        {
        }

        private void Library_NewMediaItemError(object sender, NewMediaItemErrorEventArgs e)
        {
        }

        private void Library_NewMediaItemAdded(object sender, NewMediaItemEventArgs e)
        {
            if (e.MediaItem is AlbumItem album)
            {
                if (this.Albums.Contains(album))
                {
                    return;
                }

                // If this album belongs to this artist.
                if (album.ArtistItemId == this.artistId)
                {
                    this.Albums.Add(album);
                    this.Albums.Sort((a, b) => a.Name.CompareTo(b.Name));
                }
            }
        }
    }
}
