// <copyright file="PlatformSettings.Windows.cs" company="Drastic Actions">
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
    public class PlatformSettings : IPlatformSettings
    {
        /// <inheritdoc/>
        public string DatabasePath => System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);

        /// <inheritdoc/>
        public bool IsDarkTheme
        {
            get
            {
                var uiSettings = new Windows.UI.ViewManagement.UISettings();
                var color = uiSettings.GetColorValue(Windows.UI.ViewManagement.UIColorType.Background).ToString(System.Globalization.CultureInfo.InvariantCulture);
                return color switch
                {
                    "#FF000000" => true,
                    "#FFFFFFFF" => false,
                    _ => false,
                };
            }
        }

        /// <inheritdoc/>
        public List<MediaFolder> GetDefaultMediaFolders()
        {
            return new List<MediaFolder>();
        }

        /// <inheritdoc/>
        public bool IsFileAvailable(string path)
        {
            return System.IO.File.Exists(path);
        }
    }
}
