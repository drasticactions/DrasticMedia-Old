// <copyright file="ArtistAppleMusicMetadata.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DrasticMedia.Core.Model.Metadata
{
    public class ArtistAppleMusicMetadata : IArtistMetadata
    {
        public ArtistAppleMusicMetadata()
        {
        }

        public ArtistAppleMusicMetadata(int artistId)
        {
            if (artistId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(artistId), "Must be higher than 0");
            }

            this.ArtistItemId = artistId;
            this.LastUpdated = DateTime.Now;
        }

        public string Type => this.GetType().Name;

        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Image { get; set; }

        public DateTime? LastUpdated { get; set; }

        public int ArtistItemId { get; set; }

        public virtual ArtistItem? ArtistItem { get; set; }
    }
}
