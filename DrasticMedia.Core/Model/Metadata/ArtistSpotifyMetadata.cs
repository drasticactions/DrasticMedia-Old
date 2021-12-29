// <copyright file="ArtistSpotifyMetadata.cs" company="Drastic Actions">
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
            this.LastUpdated = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtistSpotifyMetadata"/> class.
        /// </summary>
        /// <param name="artistId">Media Library Artist Id.</param>
        /// <param name="spotifyArtist">Spotify Artist.</param>
        public ArtistSpotifyMetadata(int artistId, FullArtist spotifyArtist)
        {
            if (artistId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(artistId), "Must be higher than 0");
            }

            this.ArtistItemId = artistId;
            this.SpotifyId = spotifyArtist.Id;
            this.Genres = string.Join(",", spotifyArtist.Genres);
            this.Type = spotifyArtist.Type;
            this.Name = spotifyArtist.Name;
            this.Popularity = spotifyArtist.Popularity;
            this.Image = spotifyArtist.Images.FirstOrDefault()?.Url;
            this.LastUpdated = DateTime.UtcNow;
            this.Uri = spotifyArtist.Uri;
        }

        public int Id { get; set; }

        public string? SpotifyId { get; set; }

        public string? Genres { get; set; }

        public string? Name { get; set; }

        public int Popularity { get; set; }

        public string? Type { get; set; }

        public string? Uri { get; set; }

        public string? Image { get; set; }

        public int ArtistItemId { get; set; }

        public DateTime? LastUpdated { get; set; }

        public virtual ArtistItem? ArtistItem { get; set; }
    }
}
