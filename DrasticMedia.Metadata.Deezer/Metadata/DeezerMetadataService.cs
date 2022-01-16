// <copyright file="DeezerMetadataService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Model;
using DrasticMedia.Core.Model.Metadata;
using E.Deezer;

namespace DrasticMedia.Core.Metadata
{
    public class DeezerMetadataService : IAudioMetadataService
    {
        private HttpClientHandler handler;
        private DeezerSession session;

        public DeezerMetadataService(string baseLocation, string accessToken)
        {
            this.BaseMetadataLocation = baseLocation;
            this.handler = new HttpClientHandler();
            this.session = new DeezerSession(this.handler);
            var result = this.session.Login(accessToken, CancellationToken.None).Result;
            if (!result)
            {
                throw new ArgumentException("Invalid access token");
            }
        }

        /// <inheritdoc/>
        public string BaseMetadataLocation { get; internal set; } = string.Empty;

        /// <inheritdoc/>
        public async Task<IAlbumMetadata> GetAlbumMetadataAsync(AlbumItem album, string? artistName = null)
        {
            artistName = artistName ?? album.ArtistItem?.Name;
            if (artistName is null)
            {
                return new AlbumDeezerMetadata(album.Id);
            }

            var result = await this.session.Search.FindAlbums($"{artistName} - {album.Name}", CancellationToken.None);
            if (result is null)
            {
                return new AlbumDeezerMetadata(album.Id);
            }

            if (!result.Any())
            {
                return new AlbumDeezerMetadata(album.Id);
            }

            return result.First().FromAlbum(album.Id);
        }

        /// <inheritdoc/>
        public async Task<IArtistMetadata> GetArtistMetadataAsync(ArtistItem artist)
        {
            var result = await this.session.Search.FindArtists(artist.Name, CancellationToken.None);
            if (result is null)
            {
                return new ArtistDeezerMetadata(artist.Id);
            }

            if (!result.Any())
            {
                return new ArtistDeezerMetadata(artist.Id);
            }

            return result.First().FromArtist(artist.Id);
        }
    }
}
