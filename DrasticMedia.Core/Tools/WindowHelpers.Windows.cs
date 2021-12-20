// <copyright file="WindowHelpers.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace DrasticMedia.Core.Tools
{
    /// <summary>
    /// Window Helpers.
    /// </summary>
    public static class WindowHelpers
    {
        /// <summary>
        /// Gets a value indicating whether if the current view is full screen.
        /// </summary>
        /// <returns>Bool.</returns>
        public static bool IsFullScreen => ApplicationView.GetForCurrentView().IsFullScreenMode;

        /// <summary>
        /// Gets a value indicating whether the current view is compact overlay.
        /// </summary>
        public static bool IsCompactOverlay => ApplicationView.GetForCurrentView().ViewMode == ApplicationViewMode.CompactOverlay;

        /// <summary>
        /// Gets the title bar height.
        /// </summary>
        public static double TitleBarHeight => CoreApplication.GetCurrentView().TitleBar.Height;
    }
}
