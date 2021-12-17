// <copyright file="PageBackground.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DrasticMedia.Overlays
{
    public partial class PageBackground
    {
        Microsoft.UI.Xaml.Controls.Panel? panel;
        IMauiContext? mauiContext;
        FrameworkElement? element;

        /// <inheritdoc/>
        public override bool Initialize()
        {
            if (this.pageOverlayNativeElementsInitialized)
            {
                return true;
            }

            base.Initialize();

            var nativeElement = this.Window.Content.GetNative(true);
            if (nativeElement == null)
            {
                return false;
            }

            var handler = this.Window.Handler as Microsoft.Maui.Handlers.WindowHandler;
            if (handler?.NativeView is not Microsoft.UI.Xaml.Window window)
            {
                return false;
            }

            if (handler.MauiContext == null)
            {
                return false;
            }

            this.mauiContext = handler.MauiContext;

            this.panel = window.Content as Microsoft.UI.Xaml.Controls.Panel;
            if (this.panel == null)
            {
                return false;
            }

            return this.pageOverlayNativeElementsInitialized = true;
        }

        public void SetPage(Microsoft.Maui.Controls.Page page)
        {
            if (this.panel == null || this.mauiContext == null)
            {
                return;
            }

            if (this.element != null)
            {
                this.RemovePage();
            }

            this.page = page;
            var pageHandler = page.ToHandler(this.mauiContext);
            this.element = pageHandler.NativeView;
            if (this.element != null)
            {
                this.element.SetValue(Canvas.ZIndexProperty, -1);
                this.panel.Children.Add(this.element);
            }

            this.pageSet = true;
            Microsoft.Maui.Controls.Xaml.Diagnostics.VisualDiagnostics.OnChildAdded(this, this.page, 0);
        }

        public void RemovePage()
        {
            if (this.element == null)
            {
                return;
            }

            this.panel?.Children.Remove(this.element);
            this.pageSet = false;
            Microsoft.Maui.Controls.Xaml.Diagnostics.VisualDiagnostics.OnChildRemoved(this, this.page, 0);
        }

        /// <inheritdoc/>
        public override bool Deinitialize()
        {
            this.RemovePage();
            return base.Deinitialize();
        }
    }
}
