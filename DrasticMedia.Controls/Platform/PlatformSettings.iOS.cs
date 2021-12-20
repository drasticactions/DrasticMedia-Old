// <copyright file="PlatformSettings.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Model;
using UIKit;

namespace DrasticMedia.Core.Platform
{
    /// <summary>
    /// Platform Settings.
    /// </summary>
    public class PlatformSettings : IPlatformSettings
    {
        /// <inheritdoc/>
        public string DatabasePath => Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "DrasticMediaLibrary");

        /// <inheritdoc/>
        public bool IsDarkTheme
        {
            get
            {
                // TODO: Refactor to use platform main thread check.
                var result = MainThread.InvokeOnMainThreadAsync<bool>(() =>
                {
                    if (UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
                    {
                        var currentUIViewController = GetVisibleViewController();

                        if (currentUIViewController == null)
                        {
                            return false;
                        }

                        var userInterfaceStyle = currentUIViewController.TraitCollection.UserInterfaceStyle;

                        switch (userInterfaceStyle)
                        {
                            case UIUserInterfaceStyle.Light:
                                return false;
                            case UIUserInterfaceStyle.Dark:
                                return true;
                            default:
                                throw new NotSupportedException($"UIUserInterfaceStyle {userInterfaceStyle} not supported");
                        }
                    }
                    else
                    {
                        return false;
                    }
                });
                return result.Result;
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

        private static UIViewController GetVisibleViewController()
        {
            UIViewController viewController = null;

            var window = UIApplication.SharedApplication.KeyWindow;
            if (window == null)
            {
                return null;
            }

            if (window.WindowLevel == UIWindowLevel.Normal)
            {
                viewController = window.RootViewController;
            }

            if (viewController is null)
            {
                window = UIApplication.SharedApplication
                    .Windows
                    .OrderByDescending(w => w.WindowLevel)
                    .FirstOrDefault(w => w.RootViewController != null && w.WindowLevel == UIWindowLevel.Normal);

                viewController = window?.RootViewController ?? throw new InvalidOperationException("Could not find current view controller.");
            }

            while (viewController.PresentedViewController != null)
            {
                viewController = viewController.PresentedViewController;
            }

            return viewController;
        }
    }
}
