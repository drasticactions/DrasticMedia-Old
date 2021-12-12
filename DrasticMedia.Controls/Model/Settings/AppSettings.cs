// <copyright file="AppSettings.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticMedia.Core.Model
{
    /// <summary>
    /// App Settings.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Gets or sets the id for the app settings.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use dark mode.
        /// </summary>
        public bool IsDarkMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use hardware decoding.
        /// </summary>
        public bool UseHardwareDecoding { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to force landscape mode for videos.
        /// </summary>
        public bool ForceLandscape { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to play a video in the background.
        /// </summary>
        public bool PlayVideoInBackground { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to send a notification when a new song starts playing.
        /// </summary>
        public bool NotificationWhenSongPlays { get; set; } = false;

        /// <summary>
        /// Gets or sets the accent color for the app.
        /// </summary>
        public string AccentColorHex { get; set; } = "e57a00";

        /// <summary>
        /// Gets or sets the users preference for how to handle external storage.
        /// </summary>
        public ExternalStorageSettings ExternalStorageSettings { get; set; }

        /// <summary>
        /// Gets or sets the users preference for the apps language.
        /// </summary>
        public Languages Language { get; set; }
    }
}
