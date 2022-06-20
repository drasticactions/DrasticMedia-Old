// <copyright file="MetadataExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using AppleMusicAPI.NET.Models.Resources;
using DrasticMedia.Core.Model.Metadata;

namespace DrasticMedia.Core.Metadata
{
    public static class MetadataExtensions
    {
        public static AlbumAppleMusicMetadata FromAlbum(this Album album, int albumId)
        {
            var appleAlbum = new AlbumAppleMusicMetadata(albumId);

            return appleAlbum;
        }

        public static ArtistAppleMusicMetadata FromArtist(this Artist artist, int artistId)
        {
            var appleArtist = new ArtistAppleMusicMetadata(artistId);

            return appleArtist;
        }
    }
}
