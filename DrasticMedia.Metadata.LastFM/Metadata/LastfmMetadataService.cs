// <copyright file="LastfmMetadataService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Model;
using DrasticMedia.Core.Model.Metadata;
using DrasticMedia.Core.Platform;
using DrasticMedia.Metadata;
using Hqub.Lastfm;

namespace DrasticMedia.Core.Metadata
{
    /// <summary>
    /// Last FM Metadata Service.
    /// </summary>
    public class LastfmMetadataService : IAudioMetadataService
    {
        private LastfmClient? client;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastfmMetadataService"/> class.
        /// </summary>
        /// <param name="settings"><see cref="IPlatformSettings"/>.</param>
        public LastfmMetadataService(IPlatformSettings settings)
        {
            this.Initialize(settings.MetadataPath, Core.Tools.ApiTokens.LastFMClientToken, Tools.ApiTokens.LastFMClientSecretToken);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LastfmMetadataService"/> class.
        /// </summary>
        /// <param name="baseLocation">Base Metadata Location.</param>
        /// <param name="apiKey">API Key.</param>
        /// <param name="apiSecret">API Secret.</param>
        public LastfmMetadataService(string baseLocation, string apiKey = "", string apiSecret = "")
        {
            this.Initialize(baseLocation, apiKey, apiSecret);
        }

        /// <inheritdoc/>
        public string BaseMetadataLocation { get; internal set; } = string.Empty;

        /// <inheritdoc/>
        public async Task<IArtistMetadata> GetArtistMetadataAsync(ArtistItem artist)
        {
            if (this.client is null)
            {
                return new ArtistLastFmMetadata(artist.Id);
            }

            var result = await this.client.Artist.GetInfoAsync(artist.Name);
            if (result is null)
            {
                return new ArtistLastFmMetadata(artist.Id);
            }

            return result.FromArtist(artist.Id);
        }

        /// <inheritdoc/>
        public async Task<IAlbumMetadata> GetAlbumMetadataAsync(AlbumItem album, string? artistName = null)
        {
            if (this.client is null)
            {
                return new AlbumLastFmMetadata(album.Id);
            }

            artistName = artistName ?? album.ArtistItem?.Name;
            if (artistName is null)
            {
                return new AlbumLastFmMetadata(album.Id);
            }

            var result = await this.client.Album.GetInfoAsync(artistName, album.Name);
            if (result is null)
            {
                return new AlbumLastFmMetadata(album.Id);
            }

            return result.FromAlbum(album.Id);
        }

        private void Initialize(string baseLocation, string apiKey = "", string apiSecret = "")
        {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                return;
            }

            this.client = new LastfmClient(apiKey, apiSecret);
            if (string.IsNullOrEmpty(baseLocation))
            {
                throw new ArgumentNullException(nameof(baseLocation));
            }

            var directory = Directory.CreateDirectory(baseLocation);
            if (!directory.Exists)
            {
                throw new ArgumentNullException(nameof(baseLocation));
            }

            this.BaseMetadataLocation = baseLocation;
        }
    }
}
