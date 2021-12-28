using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;

namespace DrasticMedia.Core
{
    public static class ArtistExtensions
    {
        public static async Task SaveArtistImage(this ArtistItem artist, string baseMetadataLocation, Uri uri, HttpClient client)
        {
            if (string.IsNullOrEmpty(artist.Name))
            {
                return;
            }

            var artPath = System.IO.Path.Combine(baseMetadataLocation, artist.Name, "artist.jpg");
            if (string.IsNullOrEmpty(artPath))
            {
                return;
            }

            var imageContent = await client.GetByteArrayAsync(uri);
            if (imageContent != null)
            {
                var directoryName = Path.GetDirectoryName(artPath);
                if (string.IsNullOrEmpty(directoryName))
                {
                    return;
                }

                System.IO.Directory.CreateDirectory(directoryName);
                System.IO.File.WriteAllBytes(artPath, imageContent);
                if (File.Exists(artPath))
                {
                    artist.ArtistImage = artPath;
                }
            }
        }
    }
}
