// <copyright file="VLCMediaParser.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core;
using DrasticMedia.Core.Helpers;
using DrasticMedia.Core.Library;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Platform;

namespace DrasticMedia.VLC.Library
{
    /// <summary>
    /// VLC Media Parser.
    /// </summary>
    public class VLCMediaParser : ILocalMetadataParser
    {
        private bool disposedValue;
        private LibVLCSharp.Shared.LibVLC libVLC;
        private HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="VLCMediaParser"/> class.
        /// </summary>
        /// <param name="libVLC">LibVLC.</param>
        /// <param name="platformSettings">Location to store metadata.</param>
        public VLCMediaParser(LibVLCSharp.Shared.LibVLC libVLC, IPlatformSettings platformSettings)
        {
            this.libVLC = libVLC;

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
        public Task<string> CacheAlbumImageToStorage(ArtistItem artist, AlbumItem album, string path)
            => artist.SaveAlbumImage(album, this.BaseMetadataLocation, path, this.httpClient);

        /// <inheritdoc/>
        public Task<string> CacheArtistImageToStorage(ArtistItem artist, string path)
            => artist.SaveArtistImage(this.BaseMetadataLocation, path, this.httpClient);

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public async Task<TrackItem?> GetMusicPropertiesAsync(string path) => await this.libVLC.GetMusicPropertiesAsync(path) as TrackItem;

        /// <inheritdoc/>
        public async Task<VideoItem?> GetVideoPropertiesAsync(string path) => await this.libVLC.GetVideoPropertiesAsync(path) as VideoItem;

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
    }
}
