// <copyright file="PageOverlay.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Tools;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WFlowDirection = Microsoft.UI.Xaml.FlowDirection;
using WinPoint = Windows.Foundation.Point;

namespace DrasticMedia.Overlays
{
    public partial class PageOverlay
    {
        Microsoft.UI.Xaml.Controls.Panel? panel;
        IMauiContext? mauiContext;
        FrameworkElement? element;
        IList<IView> elements = new List<IView>();

        /// <inheritdoc/>
        public override bool Initialize()
        {
            if (this.pageOverlayNativeElementsInitialized)
            {
                return true;
            }

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

            this.panel.PointerMoved += Panel_PointerMoved;
            return this.pageOverlayNativeElementsInitialized = true;
        }

        public void SetPage(Microsoft.Maui.Controls.Page page, bool toBack = false)
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
                if (toBack)
                {
                    this.element.SetValue(Canvas.ZIndexProperty, -1);
                }
                else
                {
                   this.element.SetValue(Canvas.ZIndexProperty, 100);
                }

                this.panel.Children.Add(this.element);
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
            this.panel.PointerMoved -= Panel_PointerMoved;
            return base.Deinitialize();
        }

        private void Panel_PointerMoved(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (this.element == null || !this.elements.Any())
            {
                return;
            }

            var pointerPoint = e.GetCurrentPoint(this.element);
            if (pointerPoint == null)
                return;

            this.element.IsHitTestVisible = this.elements.Any(n => n.GetBoundingBox().Contains(new Point(pointerPoint.Position.X, pointerPoint.Position.Y)));
        }
    }

    public static class VisualDiagnosticsExtensions
    {
        internal static Rectangle GetBoundingBox(this IView view)
            => view.GetNative(true).GetBoundingBox();

        internal static Rectangle GetBoundingBox(this FrameworkElement? nativeView)
        {
            if (nativeView == null)
                return new Rectangle();

            var rootView = nativeView.XamlRoot.Content;
            if (nativeView == rootView)
            {
                if (rootView is not FrameworkElement el)
                    return new Rectangle();

                return new Rectangle(0, 0, el.ActualWidth, el.ActualHeight);
            }

            var topLeft = nativeView.TransformToVisual(rootView).TransformPoint(new WinPoint());
            var topRight = nativeView.TransformToVisual(rootView).TransformPoint(new WinPoint(nativeView.ActualWidth, 0));
            var bottomLeft = nativeView.TransformToVisual(rootView).TransformPoint(new WinPoint(0, nativeView.ActualHeight));
            var bottomRight = nativeView.TransformToVisual(rootView).TransformPoint(new WinPoint(nativeView.ActualWidth, nativeView.ActualHeight));

            var x1 = new[] { topLeft.X, topRight.X, bottomLeft.X, bottomRight.X }.Min();
            var x2 = new[] { topLeft.X, topRight.X, bottomLeft.X, bottomRight.X }.Max();
            var y1 = new[] { topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y }.Min();
            var y2 = new[] { topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y }.Max();
            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }
    }
}
