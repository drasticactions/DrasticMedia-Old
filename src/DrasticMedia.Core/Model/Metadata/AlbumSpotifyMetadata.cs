// <copyright file="AlbumSpotifyMetadata.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DrasticMedia.Core.Model.Metadata
{
    public class AlbumSpotifyMetadata : IAlbumMetadata
    {
        public AlbumSpotifyMetadata()
        {
        }

        public AlbumSpotifyMetadata(int albumId)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId), "Must be higher than 0");
            }

            this.AlbumItemId = albumId;
            this.LastUpdated = DateTime.Now;
        }

        public string Type => this.GetType().Name;

        public int Id { get; set; }

        public string? AlbumType { get; set; }

        public string? Artists { get; set; }

        public string? SpotifyId { get; set; }

        public string? ReleaseDate { get; set; }

        public string? ReleaseDatePrecision { get; set; }

        public int TotalTracks { get; set; }

        public string? Uri { get; set; }

        public string? Name { get; set; }

        public string? Image { get; set; }

        public DateTime? LastUpdated { get; set; }

        public int AlbumItemId { get; set; }

        public virtual AlbumItem? AlbumItem { get; set; }
    }
}
