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

namespace DrasticMedia.Core.Library
{
    /// <summary>
    /// Native Media Parser.
    /// </summary>
    public class NativeMediaParser : ILocalMetadataParser
    {
        private bool disposedValue;

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public async Task<TrackItem?> GetMusicPropertiesAsync(string path)
        {
            return new TrackItem() { Path = path };
        }

        /// <inheritdoc/>
        public async Task<VideoItem?> GetVideoPropertiesAsync(string path)
        {
            return new VideoItem() { Path = path };
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
    }
}
