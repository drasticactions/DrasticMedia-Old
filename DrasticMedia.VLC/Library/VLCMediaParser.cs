// <copyright file="VLCMediaParser.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Helpers;
using DrasticMedia.Core.Library;
using DrasticMedia.Core.Model;

namespace DrasticMedia.VLC.Library
{
    /// <summary>
    /// VLC Media Parser.
    /// </summary>
    public class VLCMediaParser : ILocalMetadataParser
    {
        private bool disposedValue;
        private LibVLCSharp.Shared.LibVLC libVLC;

        /// <summary>
        /// Initializes a new instance of the <see cref="VLCMediaParser"/> class.
        /// </summary>
        /// <param name="libVLC">LibVLC.</param>
        public VLCMediaParser(LibVLCSharp.Shared.LibVLC libVLC)
        {
            this.libVLC = libVLC;
        }

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
