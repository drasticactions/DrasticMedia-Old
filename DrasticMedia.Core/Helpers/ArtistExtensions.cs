using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Utilities;

namespace DrasticMedia.Core
{
    public static class ArtistExtensions
    {
        public static async Task<string> SaveAlbumImage (this ArtistItem artist, AlbumItem album, string baseMetadataLocation, string path, HttpClient client)
        {
            if (artist.Name is null || album.Name is null)
            {
                return string.Empty;
            }

            var albumArtPath = System.IO.Path.Combine(baseMetadataLocation, artist.Name.CleanPath(), album.Name.CleanPath(), "album.jpg");
            if (System.IO.File.Exists(albumArtPath))
            {
                return albumArtPath;
            }

            var directory = Path.GetDirectoryName(albumArtPath);
            if (directory is null)
            {
                return string.Empty;
            }

            System.IO.Directory.CreateDirectory(directory);

            try
            {
                if (path.IsPathUri())
                {
                    var result = await client.GetByteArrayAsync(path);
                    await File.WriteAllBytesAsync(albumArtPath, result);
                    if (File.Exists(albumArtPath))
                    {
                        return albumArtPath;
                    }

                    return string.Empty;
                }

                var file = await File.ReadAllBytesAsync(path);
                await File.WriteAllBytesAsync(albumArtPath, file);
                if (File.Exists(albumArtPath))
                {
                    return albumArtPath;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public static async Task<string> SaveArtistImage(this ArtistItem artist, string baseMetadataLocation, string path, HttpClient client)
        {
            if (artist.Name is null)
            {
                return string.Empty;
            }

            var artistArtPath = System.IO.Path.Combine(baseMetadataLocation, artist.Name.CleanPath(), "artist.jpg");
            if (System.IO.File.Exists(artistArtPath))
            {
                return artistArtPath;
            }

            var directory = Path.GetDirectoryName(artistArtPath);
            if (directory is null)
            {
                return string.Empty;
            }

            System.IO.Directory.CreateDirectory(directory);

            try
            {
                if (path.IsPathUri())
                {
                    var result = await client.GetByteArrayAsync(path);
                    await File.WriteAllBytesAsync(artistArtPath, result);
                    if (File.Exists(artistArtPath))
                    {
                        return artistArtPath;
                    }

                    return string.Empty;
                }

                var file = await File.ReadAllBytesAsync(path);
                await File.WriteAllBytesAsync(artistArtPath, file);
                if (File.Exists(artistArtPath))
                {
                    return artistArtPath;
                }

                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
