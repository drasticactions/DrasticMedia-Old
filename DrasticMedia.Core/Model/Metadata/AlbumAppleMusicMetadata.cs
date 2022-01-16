// <copyright file="AlbumAppleMusicMetadata.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DrasticMedia.Core.Model.Metadata
{
    public class AlbumAppleMusicMetadata : IAlbumMetadata
    {
        public AlbumAppleMusicMetadata()
        {
        }

        public AlbumAppleMusicMetadata(int albumId)
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

        public string? Name { get; set; }

        public string? Image { get; set; }

        public DateTime? LastUpdated { get; set; }

        public int AlbumItemId { get; set; }

        public virtual AlbumItem? AlbumItem { get; set; }
    }
}
