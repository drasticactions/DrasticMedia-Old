// <copyright file="AlbumLastFmMetadata.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DrasticMedia.Core.Model.Metadata
{
    public class AlbumLastFmMetadata : IAlbumMetadata
    {
        public AlbumLastFmMetadata()
        {
            this.LastUpdated = DateTime.Now;
        }

        public AlbumLastFmMetadata(int albumId, Hqub.Lastfm.Entities.Album album)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId), "Must be higher than 0");
            }

            this.AlbumItemId = albumId;
            this.MBID = album.MBID;
            this.Name = album.Name;
            this.Image = album.Images.LastOrDefault()?.Url;
            this.LastUpdated = DateTime.UtcNow;
        }

        public string Type => this.GetType().Name;

        public int Id { get; set; }

        public string? MBID { get; set; }

        public string? Name { get; set; }

        public string? Image { get; set; }

        public DateTime? LastUpdated { get; set; }

        public int AlbumItemId { get; set; }

        public virtual AlbumItem? AlbumItem { get; set; }
    }
}
