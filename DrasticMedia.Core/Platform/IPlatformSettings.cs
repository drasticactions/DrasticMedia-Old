// <copyright file="IPlatformSettings.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;

namespace DrasticMedia.Core.Platform
{
    /// <summary>
    /// Platform Settings.
    /// </summary>
    public interface IPlatformSettings
    {
        /// <summary>
        /// Gets the path to where the database is stored.
        /// </summary>
        string DatabasePath { get; }

        /// <summary>
        /// Gets a value indicating whether the current platform is running a system level dark theme.
        /// </summary>
        bool IsDarkTheme { get; }

        /// <summary>
        /// Gets a value indicating whether the file is available to access.
        /// </summary>
        /// <param name="path">File Path.</param>
        /// <returns>Bool.</returns>
        bool IsFileAvailable(string path);

        /// <summary>
        /// Gets the default media folders for the platform.
        /// </summary>
        /// <returns>List of MediaFolder.</returns>
        List<MediaFolder> GetDefaultMediaFolders();
    }
}
