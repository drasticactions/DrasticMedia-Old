// <copyright file="MockPlatformSettings.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Platform;

namespace DrasticMedia.LiteDB.Native.Test
{
    /// <summary>
    /// Mock platform Settings.
    /// </summary>
    public class MockPlatformSettings : IPlatformSettings
    {
        /// <inheritdoc/>
        public string DatabasePath => string.Empty;

        /// <inheritdoc/>
        public bool IsDarkTheme => false;

        /// <inheritdoc/>
        public string MetadataPath => string.Empty;

        /// <inheritdoc/>
        public List<MediaFolder> GetDefaultMediaFolders() => new List<MediaFolder>();

        /// <inheritdoc/>
        public bool IsFileAvailable(string path) => System.IO.File.Exists(path);
    }
}