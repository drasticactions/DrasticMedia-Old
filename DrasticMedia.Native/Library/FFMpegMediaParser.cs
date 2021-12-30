// <copyright file="NativeMediaParser.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Library;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Platform;
using DrasticMedia.Core.Utilities;
using FFMpegCore;
using Orthogonal.NTagLite;

namespace DrasticMedia.Core.Library
{
    /// <summary>
    /// FFMpeg Media Parser.
    /// </summary>
    public class FFMpegMediaParser : ILocalMetadataParser
    {
        private bool disposedValue;
        private HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="FFMpegMediaParser"/> class.
        /// </summary>
        /// <param name="baseLocation">Location to store metadata.</param>
        public FFMpegMediaParser(string baseLocation)
        {
            if (string.IsNullOrEmpty(baseLocation))
            {
                throw new ArgumentNullException(nameof(baseLocation));
            }

            var directory = Directory.CreateDirectory(baseLocation);
            if (!directory.Exists)
            {
                throw new ArgumentNullException(nameof(baseLocation));
            }

            this.BaseMetadataLocation = baseLocation;
            this.httpClient = new HttpClient();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FFMpegMediaParser"/> class.
        /// </summary>
        /// <param name="platformSettings">Location to store metadata.</param>
        public FFMpegMediaParser(IPlatformSettings platformSettings)
        {
            if (platformSettings == null)
            {
                throw new ArgumentNullException(nameof(platformSettings));
            }

            var directory = Directory.CreateDirectory(platformSettings.MetadataPath);
            if (!directory.Exists)
            {
                throw new ArgumentNullException(nameof(platformSettings.MetadataPath));
            }

            this.BaseMetadataLocation = platformSettings.MetadataPath;
            this.httpClient = new HttpClient();
        }

        /// <inheritdoc/>
        public string BaseMetadataLocation { get; }

        /// <inheritdoc/>
        public async Task<string> CacheAlbumImageToStorage(ArtistItem artist, AlbumItem album, string path)
        {
            if (artist.Name is null || album.Name is null)
            {
                return string.Empty;
            }

            var albumArtPath = System.IO.Path.Combine(this.BaseMetadataLocation, artist.Name.CleanPath(), album.Name.CleanPath(), "album.jpg");
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
                    var result = await this.httpClient.GetByteArrayAsync(path);
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

        /// <inheritdoc/>
        public async Task<string> CacheArtistImageToStorage(ArtistItem artist, string path)
        {
            if (artist.Name is null)
            {
                return string.Empty;
            }

            var artistArtPath = System.IO.Path.Combine(this.BaseMetadataLocation, artist.Name.CleanPath(), "artist.jpg");
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
                    var result = await this.httpClient.GetByteArrayAsync(path);
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

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public async Task<TrackItem?> GetMusicPropertiesAsync(string path)
        {
            var file = LiteFile.LoadFromFile(path);

            if (string.IsNullOrEmpty(file.Tag.Id))
            {
                return await this.GetMusicPropertiesViaFFMpegAsync(path);
            }

            var albumArt = await this.ParseAlbumArt(file);

            return new TrackItem()
            {
                Artist = file.Tag.Artist,
                Album = file.Tag.Album,
                Title = file.Tag.Title,
                Year = file.Tag.Year is not null ? (int)file.Tag.Year : 0,
                Tracknumber = file.Tag.TrackNumber is not null ? (uint)file.Tag.TrackNumber : 0,
                AlbumArt = albumArt,
                Path = path,
            };
        }

        /// <inheritdoc/>
        public async Task<VideoItem?> GetVideoPropertiesAsync(string path)
        {
            var mediainfo = await FFProbe.AnalyseAsync(path);
            return new VideoItem()
            {
                Width = (uint)(mediainfo.PrimaryVideoStream?.Width ?? 0),
                Height = (uint)(mediainfo.PrimaryVideoStream?.Height ?? 0),
                Duration = mediainfo.PrimaryVideoStream?.Duration ?? TimeSpan.Zero,
                Path = path,
            };
        }

        private async Task<TrackItem?> GetMusicPropertiesViaFFMpegAsync(string path)
        {
            var mediainfo = await FFProbe.AnalyseAsync(path);
            var format = mediainfo.Format;
            if (format?.Tags is null)
            {
                throw new NullReferenceException($"Could not parse {path}");
            }

            var year = format.Tags.ContainsKey("date") ? Convert.ToInt32(format.Tags["date"]) : 0;
            year = year <= 0 && format.Tags.ContainsKey("WM/Year") ? Convert.ToInt32(format.Tags["WM/Year"]) : 0;

            return new TrackItem()
            {
                Artist = format.Tags["artist"],
                Album = format.Tags["album"],
                Title = format.Tags["title"],
                Year = year,
                Tracknumber = format.Tags["track"] is not null ? (uint)Convert.ToInt32(format.Tags["track"]) : 0,
                Path = path,
            };
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Is Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                }

                this.disposedValue = true;
            }
        }

        private async Task<string> ParseAlbumArt(LiteFile file)
        {
            var albumArtPath = System.IO.Path.Combine(this.BaseMetadataLocation, file.Tag.Artist.CleanPath(), file.Tag.Album.CleanPath(), "album.jpg");
            if (System.IO.File.Exists(albumArtPath))
            {
                return albumArtPath;
            }

            Picture[] pics = file.Tag.FindFramesById(FrameId.APIC).Select(f => f.GetPicture()).ToArray();
            var front = pics.SingleOrDefault(p => p.PictureType == LitePictureType.CoverFront);
            if (front is null)
            {
                return string.Empty;
            }

            var directory = Path.GetDirectoryName(albumArtPath);
            if (directory is null)
            {
                return string.Empty;
            }

            System.IO.Directory.CreateDirectory(directory);
            await System.IO.File.WriteAllBytesAsync(albumArtPath, front.Data);

            return albumArtPath;
        }
    }
}
