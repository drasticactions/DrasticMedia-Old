// <copyright file="PageOverlay.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Tools;
using UIKit;

namespace DrasticMedia.Overlays
{
    /// <summary>
    /// Page Overlay.
    /// </summary>
    public partial class PageOverlay
    {
        private UIWindow? window;
        private IMauiContext? mauiContext;
        private UIView? element;
        private IList<IView> elements = new List<IView>();

        /// <inheritdoc/>
        public override bool Initialize()
        {
            if (this.pageOverlayNativeElementsInitialized)
            {
                return true;
            }

            base.Initialize();

            var handler = this.Window?.Handler;
            if (handler == null)
            {
                return false;
            }

            this.mauiContext = handler.MauiContext;

            var nativeLayer = this.Window?.GetNative(true);
            if (nativeLayer is not UIWindow nativeWindow)
            {
                return false;
            }

            if (nativeWindow?.RootViewController?.View == null)
            {
                return false;
            }

            this.window = nativeWindow;

            return this.pageOverlayNativeElementsInitialized = true;
        }

        /// <inheritdoc/>
        public void SetPage(Page page, bool toBack = false, int zindex = 0)
        {
            if (this.window?.RootViewController?.View == null || this.mauiContext == null)
            {
                return;
            }

            var view = page.ToHandler(this.mauiContext);
            this.element = view.NativeView;
            if (this.element != null)
            {
                this.element.AutoresizingMask = UIViewAutoresizing.All;
                this.window?.RootViewController.View.AddSubview(this.element);
                if (toBack)
                {
                    this.window?.RootViewController.View.SendSubviewToBack(this.element);
                }
                else
                {
                    this.window?.RootViewController.View.BringSubviewToFront(this.element);
                }
            }

            if (page is IHitTestPage hitTestPage)
            {
                foreach (var htElement in hitTestPage.HitTestViews)
                {
                    this.elements.Add(htElement);
                }
            }

            this.pageSet = true;
            Microsoft.Maui.Controls.Xaml.Diagnostics.VisualDiagnostics.OnChildAdded(this, this.page, 0);
        }

        /// <inheritdoc/>
        public void RemovePage()
        {
            if (this.element == null)
            {
                return;
            }

            this.element.RemoveFromSuperview();
            this.element.Dispose();
            this.pageSet = false;

            Microsoft.Maui.Controls.Xaml.Diagnostics.VisualDiagnostics.OnChildRemoved(this, this.page, 0);
        }
    }
}
