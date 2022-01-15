// <copyright file="MediaFolder.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DrasticMedia.Core.Model
{
    /// <summary>
    /// Media Folder.
    /// </summary>
    public class MediaFolder
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the id for the app settings.
        /// </summary>
        public int AppSettingsId { get; set; }

        /// <summary>
        /// Gets or sets the path for the folder.
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of media folder this is.
        /// </summary>
        public MediaFolderType MediaFolderType { get; set; }
    }
}
