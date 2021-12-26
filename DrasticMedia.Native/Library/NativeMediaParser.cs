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
using FFMpegCore;
using Orthogonal.NTagLite;

namespace DrasticMedia.Core.Library
{
    /// <summary>
    /// Native Media Parser.
    /// </summary>
    public class NativeMediaParser : ILocalMetadataParser
    {
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeMediaParser"/> class.
        /// </summary>
        /// <param name="baseLocation">Location to store metadata.</param>
        public NativeMediaParser(string baseLocation)
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
        }

        /// <inheritdoc/>
        public string BaseMetadataLocation { get; }

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
            var albumArtPath = System.IO.Path.Combine(this.BaseMetadataLocation, file.Tag.Artist, file.Tag.Album, "album.jpg");
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
