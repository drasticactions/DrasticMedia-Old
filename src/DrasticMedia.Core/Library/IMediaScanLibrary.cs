// <copyright file="IMediaScanLibrary.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DrasticMedia.Core.Library
{
    /// <summary>
    /// Media Scan.
    /// </summary>
    public interface IMediaScanLibrary
    {
        /// <summary>
        /// Recursivly scan media directories.
        /// </summary>
        /// <param name="mediaDirectory">Starting directory.</param>
        /// <returns>Task.</returns>
        Task ScanMediaDirectoriesAsync(string mediaDirectory);

        /// <summary>
        /// Scan media directory.
        /// </summary>
        /// <param name="mediaDirectory">Directory.</param>
        /// <returns>Task.</returns>
        Task ScanMediaDirectoryAsync(string mediaDirectory);
    }
}
