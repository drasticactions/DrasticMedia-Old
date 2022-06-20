// <copyright file="ArtistLastFmMetadata.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DrasticMedia.Core.Model.Metadata
{
    public class ArtistLastFmMetadata : IArtistMetadata
    {
        public ArtistLastFmMetadata()
        {
        }

        public ArtistLastFmMetadata(int artistId)
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

        public string? Biography { get; set; }

        public string? BiographyPublished { get; set; }

        public string? MBID { get; set; }

        public DateTime? LastUpdated { get; set; }

        public int ArtistItemId { get; set; }

        public virtual ArtistItem? ArtistItem { get; set; }
    }
}
