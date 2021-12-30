// <copyright file="ArtistListPageViewModel.cs" company="Drastic Actions">
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
using DrasticMedia.Core.Utilities;
using DrasticMedia.Utilities;

namespace DrasticMedia.ViewModels
{
    public class ArtistListPageViewModel : BaseViewModel
    {
        private AsyncCommand refreshMusicLibraryCommand;

        public ArtistListPageViewModel(IServiceProvider services, Page originalPage = null)
            : base(services, originalPage)
        {
            this.MediaLibrary.NewMediaItemAdded += this.Library_NewMediaItemAdded;
            this.MediaLibrary.NewMediaItemError += this.Library_NewMediaItemError;
            this.MediaLibrary.RemoveMediaItem += this.Library_RemoveMediaItem;
            this.MediaLibrary.UpdateMediaItemAdded += this.Library_UpdateMediaItemAdded;
        }

        /// <summary>
        /// Gets the refresh music library command.
        /// </summary>
        public AsyncCommand RefreshMusicLibraryCommand
        {
            get
            {
                return this.refreshMusicLibraryCommand ??= new AsyncCommand(this.RefreshMusicLibrary, null, this.Error);
            }
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

        private async Task RefreshMusicLibrary()
        {
            this.MediaLibrary.ScanMediaDirectoriesAsync(@"C:\Users\t_mil\Music").FireAndForgetSafeAsync();
        }

        private async Task RefreshArtists()
        {
            this.Artists.Clear();
            var artists = (await this.MediaLibrary.FetchArtistsAsync()).OrderBy(n => n.Name);
            foreach (var artist in artists)
            {
                this.Artists.Add(artist);
            }
        }

        private void Library_UpdateMediaItemAdded(object sender, UpdateMediaItemEventArgs e)
        {
            //if (e.MediaItem is ArtistItem artist)
            //{
            //    if (this.Artists.Any())
            //}
        }

        private void Library_RemoveMediaItem(object sender, RemoveMediaItemEventArgs e)
        {
        }

        private void Library_NewMediaItemError(object sender, NewMediaItemErrorEventArgs e)
        {
        }

        private void Library_NewMediaItemAdded(object sender, NewMediaItemEventArgs e)
        {
            if (e.MediaItem is ArtistItem artist)
            {
                // If, for some reason, we already have the artist. Return.
                if (this.Artists.Contains(artist))
                {
                    return;
                }

                this.Artists.Add(artist);
                this.Artists.Sort((a, b) => a.Name.CompareTo(b.Name));
            }
        }
    }
}
