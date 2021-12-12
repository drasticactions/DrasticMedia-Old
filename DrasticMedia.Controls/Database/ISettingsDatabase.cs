// <copyright file="ISettingsDatabase.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Model;

namespace DrasticMedia.Core.Database
{
    /// <summary>
    /// Settings Database.
    /// </summary>
    public interface ISettingsDatabase : IDatabase
    {
        /// <summary>
        /// Fetches the app settings. If they don't exist,
        /// new defaults will be created.
        /// </summary>
        /// <returns>AppSettings.</returns>
        Task<AppSettings> FetchAppSettingsAsync();

        /// <summary>
        /// Saves app settings.
        /// </summary>
        /// <param name="settings">AppSettings.</param>
        /// <returns>New AppSettings.</returns>
        Task<AppSettings> SaveAppSettingsAsync(AppSettings settings);

        /// <summary>
        /// Fetches all media folders.
        /// </summary>
        /// <returns>List of MediaFolder.</returns>
        Task<List<MediaFolder>> FetchMediaFoldersAsync();

        /// <summary>
        /// Saves MediaFolder.
        /// </summary>
        /// <param name="folder">MediaFolder.</param>
        /// <returns>New MediaFolder.</returns>
        Task<MediaFolder> SaveMediaFolderAsync(MediaFolder folder);

        /// <summary>
        /// Saves MediaFolder.
        /// </summary>
        /// <param name="folders">MediaFolders.</param>
        /// <returns>List of MediaFolder.</returns>
        Task<List<MediaFolder>> SaveMediaFoldersAsync(List<MediaFolder> folders);
    }
}
