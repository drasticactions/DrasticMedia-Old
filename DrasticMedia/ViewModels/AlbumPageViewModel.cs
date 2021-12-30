// <copyright file="AlbumPageViewModel.cs" company="Drastic Actions">
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
    public class AlbumPageViewModel : BaseViewModel
    {
        private int albumId;
        private AlbumItem? album;
        private AsyncCommand<TrackItem>? playTrackCommand;
        private AsyncCommand? playAlbumCommand;
        private PlayerService? playerService;

        public AlbumPageViewModel(IServiceProvider services, Page originalPage = null, int albumId = 0)
            : base(services, originalPage)
        {
            this.albumId = albumId;
            this.playerService = services.GetService<PlayerService>();
            if (this.playerService == null)
            {
                throw new ArgumentNullException(nameof(this.playerService));
            }
        }

        /// <summary>
        /// Gets the add navigate to podcast command.
        /// </summary>
        public AsyncCommand<TrackItem> PlayTrackCommand
        {
            get
            {
                return this.playTrackCommand ??= new AsyncCommand<TrackItem>(this.PlayTrack, null, this.Error);
            }
        }

        /// <summary>
        /// Gets the add navigate to podcast command.
        /// </summary>
        public AsyncCommand PlayAlbumCommand
        {
            get
            {
                return this.playAlbumCommand ??= new AsyncCommand(this.PlayAlbum, null, this.Error);
            }
        }

        /// <summary>
        /// Gets the podcast.
        /// </summary>
        public AlbumItem? Album => this.album;

        /// <summary>
        /// Gets the list of episodes.
        /// </summary>
        public List<TrackItem>? Tracks => this.album?.Tracks ?? new List<TrackItem>();

        /// <inheritdoc/>
        public override async Task LoadAsync()
        {
            await base.LoadAsync();
            if (this.album == null && this.albumId > 0)
            {
                await this.LoadAlbum(this.albumId);
            }
        }

        /// <summary>
        /// Load Artist.
        /// </summary>
        /// <param name="artistId">Artist Id.</param>
        /// <returns>Task.</returns>
        public async Task LoadAlbum(int albumId)
        {
            this.albumId = albumId;
            this.album = await this.MediaLibrary.FetchAlbumWithTracksViaIdAsync(albumId);
            this.OnPropertyChanged(nameof(this.Album));
            this.OnPropertyChanged(nameof(this.Tracks));
        }

        private async Task PlayAlbum()
        {
        }

        private async Task PlayTrack(TrackItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await this.playerService.AddMedia(item, true);
        }
    }
}
