// <copyright file="AlbumSpotifyMetadata.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;

namespace DrasticMedia.Core.Model.Metadata
{
    public class AlbumSpotifyMetadata : IAlbumMetadata
    {
        public AlbumSpotifyMetadata()
        {
            this.LastUpdated = DateTime.Now;
        }

        public AlbumSpotifyMetadata(int albumId, SimpleAlbum spotifyAlbum)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId), "Must be higher than 0");
            }

            this.AlbumItemId = albumId;
            this.AlbumType = spotifyAlbum.AlbumType;
            this.Type = spotifyAlbum.Type;
            this.TotalTracks = spotifyAlbum.TotalTracks;
            this.Name = spotifyAlbum.Name;
            this.Uri = spotifyAlbum.Uri;
            this.Artists = string.Join(", ", spotifyAlbum.Artists.Select(n => n.Name));
            this.Image = spotifyAlbum.Images.FirstOrDefault()?.Url;
            this.ReleaseDate = spotifyAlbum.ReleaseDate;
            this.ReleaseDatePrecision = spotifyAlbum.ReleaseDatePrecision;
            this.SpotifyId = spotifyAlbum.Id;
            this.LastUpdated = DateTime.UtcNow;
        }

        public int Id { get; set; }

        public string? AlbumType { get; set; }

        public string? Artists { get; set; }

        public string? SpotifyId { get; set; }

        public string? ReleaseDate { get; set; }

        public string? ReleaseDatePrecision { get; set; }

        public int TotalTracks { get; set; }

        public string? Type { get; set; }

        public string? Uri { get; set; }

        public string? Name { get; set; }

        public string? Image { get; set; }

        public DateTime? LastUpdated { get; set; }

        public int AlbumItemId { get; set; }

        public virtual AlbumItem? AlbumItem { get; set; }
    }
}
