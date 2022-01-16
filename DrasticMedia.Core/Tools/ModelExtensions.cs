// <copyright file="ModelExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Model;

namespace DrasticMedia.Core.Tools
{
    public static class ModelExtensions
    {
        public static string? AlbumUrlFromMetadata(this AlbumItem item)
        {
            return item.SpotifyMetadata?.Image;
        }

        public static string? ArtistUrlFromMetadata(this ArtistItem item)
        {
            return item.SpotifyMetadata?.Image;
        }
    }
}
