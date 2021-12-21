// <copyright file="PlatformSettings.Android.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Android.Content.Res;
using Android.OS;
using DrasticMedia.Core.Model;

namespace DrasticMedia.Core.Platform
{
    /// <summary>
    /// Platform Settings.
    /// </summary>
    public class PlatformSettings : IPlatformSettings
    {
        /// <inheritdoc/>
        public string DatabasePath => Path.Combine(Microsoft.Maui.Essentials.FileSystem.AppDataDirectory, "DrasticMediaLibrary");

        /// <inheritdoc/>
        public bool IsDarkTheme
        {
            get
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Froyo)
                {
                    if (Microsoft.Maui.Essentials.Platform.CurrentActivity?.Resources?.Configuration == null)
                    {
                        return false;
                    }

                    var uiModeFlags = Microsoft.Maui.Essentials.Platform.CurrentActivity.Resources.Configuration.UiMode & UiMode.NightMask;

                    switch (uiModeFlags)
                    {
                        case UiMode.NightYes:
                            return true;

                        case UiMode.NightNo:
                            return false;
                        default:
                            return false;
                    }
                }
                else
                {
                    return false;
                }
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
