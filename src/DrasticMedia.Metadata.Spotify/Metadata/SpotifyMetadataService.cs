﻿// <copyright file="SpotifyMetadataService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Model;
using DrasticMedia.Core.Model.Metadata;
using DrasticMedia.Metadata;
using SpotifyAPI.Web;

namespace DrasticMedia.Core.Metadata
{
    /// <summary>
    /// Spotify Metadata Service.
    /// </summary>
    public class SpotifyMetadataService : IAudioMetadataService
    {
        private SpotifyClient? client;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyMetadataService"/> class.
        /// </summary>
        /// <param name="baseLocation">Base Location.</param>
        /// <param name="apiKey">API Key.</param>
        /// <param name="apiSecret">API Secret.</param>
        public SpotifyMetadataService(string baseLocation, string apiKey = "", string apiSecret = "")
        {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                return;
            }

            var config = SpotifyClientConfig.CreateDefault();
            var request = new ClientCredentialsRequest(apiKey, apiSecret);
            var response = new OAuthClient(config).RequestToken(request).Result;

            this.client = new SpotifyClient(config.WithToken(response.AccessToken));

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

        /// <inheritdoc/>
        public string BaseMetadataLocation { get; internal set; } = string.Empty;

        /// <inheritdoc/>
        public async Task<IArtistMetadata> GetArtistMetadataAsync(ArtistItem artist)
        {
            if (this.client is null)
            {
                return new ArtistSpotifyMetadata(artist.Id);
            }

            if (artist.Name is null)
            {
                return new ArtistSpotifyMetadata(artist.Id);
            }

            var result = await this.client.Search.Item(new SearchRequest(SearchRequest.Types.Artist, artist.Name));

            var artistList = result.Artists.Items;

            if (artistList is not null && artistList.Any())
            {
                var artistInfo = artistList.First();
                if (artistInfo is null)
                {
                    return new ArtistSpotifyMetadata(artist.Id);
                }

                return artistInfo.FromFullArtist(artist.Id);
            }

            return new ArtistSpotifyMetadata(artist.Id);
        }

        /// <inheritdoc/>
        public async Task<IAlbumMetadata> GetAlbumMetadataAsync(AlbumItem album, string? artistName = null)
        {
            if (this.client is null)
            {
                return new AlbumSpotifyMetadata(album.Id);
            }

            artistName = artistName ?? album.ArtistItem?.Name;
            if (artistName is null)
            {
                return new AlbumSpotifyMetadata(album.Id);
            }

            var result = await this.client.Search.Item(new SearchRequest(SearchRequest.Types.Album, $"{artistName} - {album.Name}"));

            var albumList = result.Albums.Items;

            if (albumList is not null && albumList.Any())
            {
                var albumInfo = albumList.First();
                if (albumInfo is null)
                {
                    return new AlbumSpotifyMetadata(album.Id);
                }

                return albumInfo.FromSimpleAlbum(album.Id);
            }

            return new AlbumSpotifyMetadata(album.Id);
        }
    }
}
