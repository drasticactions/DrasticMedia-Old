// <copyright file="ILocalMetadataParser.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;

namespace DrasticMedia.Core.Library
{
    /// <summary>
    /// Local Metadata Parser.
    /// </summary>
    public interface ILocalMetadataParser : IDisposable
    {
        /// <summary>
        /// Get the music properties for a given item path.
        /// </summary>
        /// <param name="path">Path to item.</param>
        /// <returns><see cref="TrackItem"/>.</returns>
        Task<TrackItem?> GetMusicPropertiesAsync(string path);

        /// <summary>
        /// Get the video properties for a given item path.
        /// </summary>
        /// <param name="path">Path to item.</param>
        /// <returns><see cref="VideoItem"/>.</returns>
        Task<VideoItem?> GetVideoPropertiesAsync(string path);
    }
}
