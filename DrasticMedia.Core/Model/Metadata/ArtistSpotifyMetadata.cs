// <copyright file="ArtistSpotifyMetadata.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DrasticMedia.Core.Model.Metadata
{
    /// <summary>
    /// Spotify Artist Metadata.
    /// </summary>
    public class ArtistSpotifyMetadata : IArtistMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArtistSpotifyMetadata"/> class.
        /// </summary>
        public ArtistSpotifyMetadata()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtistSpotifyMetadata"/> class.
        /// </summary>
        public ArtistSpotifyMetadata(int artistId)
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

        public string? SpotifyId { get; set; }

        public string? Genres { get; set; }

        public string? Name { get; set; }

        public int Popularity { get; set; }

        public string? Uri { get; set; }

        public string? Image { get; set; }

        public int ArtistItemId { get; set; }

        public DateTime? LastUpdated { get; set; }

        public virtual ArtistItem? ArtistItem { get; set; }
    }
}
