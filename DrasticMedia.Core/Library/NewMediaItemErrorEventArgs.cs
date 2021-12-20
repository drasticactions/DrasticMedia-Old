// <copyright file="NewMediaItemErrorEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DrasticMedia.Core.Library
{
    /// <summary>
    /// New Media Item Error Event Args.
    /// </summary>
    public class NewMediaItemErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the exception to parsing the media.
        /// </summary>
        public Exception? Exception { get; set; }

        /// <summary>
        /// Gets or sets the media item path.
        /// </summary>
        public string? MediaItemPath { get; set; }

        /// <summary>
        /// Gets or sets the media type.
        /// </summary>
        public MediaType MediaType { get; set; } = MediaType.Unknown;
    }
}
