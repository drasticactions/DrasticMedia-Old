// <copyright file="AppleMusicMetadataService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using AppleMusicAPI.NET.Clients;
using AppleMusicAPI.NET.Models.Enums;
using AppleMusicAPI.NET.Utilities;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Model.Metadata;

namespace DrasticMedia.Core.Metadata
{
    public class AppleMusicMetadataService : IAudioMetadataService
    {
        private JwtProviderConfiguration config;
        private JwtProvider provider;
        private JsonSerializer serializer;
        private HttpClient client;
        private CatalogClient catalogClient;
        private string storefront;

        public AppleMusicMetadataService(string baseLocation, string keyId, string teamId, string privateKeyPath, string storefront = "us")
        {
            this.storefront = storefront;
            this.BaseMetadataLocation = baseLocation;
            this.config = new JwtProviderConfiguration
            {
                KeyId = keyId,
                TeamId = teamId,
                PrivateKeyPath = privateKeyPath,
            };
            this.provider = new JwtProvider(this.config);
            this.serializer = new JsonSerializer();
            this.client = new HttpClient();
            this.catalogClient = new CatalogClient(this.client, this.serializer, this.provider);
        }

        /// <inheritdoc/>
        public string BaseMetadataLocation { get; internal set; } = string.Empty;

        /// <inheritdoc/>
        public async Task<IAlbumMetadata> GetAlbumMetadataAsync(AlbumItem album, string? artistName = null)
        {
            artistName = artistName ?? album.ArtistItem?.Name;
            if (artistName is null)
            {
                return new AlbumAppleMusicMetadata(album.Id);
            }

            var result = await this.catalogClient.CatalogResourcesSearch(this.storefront, $"{artistName} - {album.Name}", new List<ResourceType>() { ResourceType.Albums });
            if (result is null)
            {
                return new AlbumAppleMusicMetadata(album.Id);
            }

            if (!result.Results.Albums.Data.Any())
            {
                return new AlbumAppleMusicMetadata(album.Id);
            }

            return result.Results.Albums.Data.First().FromAlbum(album.Id);
        }

        /// <inheritdoc/>
        public async Task<IArtistMetadata> GetArtistMetadataAsync(ArtistItem artist)
        {
            var result = await this.catalogClient.CatalogResourcesSearch(this.storefront, $"{artist.Name}", new List<ResourceType>() { ResourceType.Artists });
            if (result is null)
            {
                return new ArtistAppleMusicMetadata(artist.Id);
            }

            if (!result.Results.Albums.Data.Any())
            {
                return new ArtistAppleMusicMetadata(artist.Id);
            }

            return result.Results.Artists.Data.First().FromArtist(artist.Id);
        }
    }
}
