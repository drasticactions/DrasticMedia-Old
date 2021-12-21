// <copyright file="PageOverlay.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CoreAnimation;
using CoreGraphics;
using DrasticMedia.Core.Tools;
using ObjCRuntime;
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
            if (view.NativeView == null)
            {
                return;
            }

            if (this.window?.RootViewController == null)
            {
                return;
            }

            this.element = new PassthroughView(this, this.window.RootViewController.View.Frame);
            if (this.element != null)
            {
                view.NativeView.Frame = this.element.Frame;
                view.NativeView.AutoresizingMask = UIViewAutoresizing.All;
                this.element.AddSubview(view.NativeView);
                this.element.BringSubviewToFront(view.NativeView);
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

        class PassthroughView : UIView
        {
            /// <summary>
            /// Event Handler for handling on touch events on the Passthrough View.
            /// </summary>
            public event EventHandler<CGPoint>? OnTouch;

            PageOverlay overlay;

            /// <summary>
            /// Initializes a new instance of the <see cref="PassthroughView"/> class.
            /// </summary>
            /// <param name="overlay">The Window Overlay.</param>
            /// <param name="frame">Base Frame.</param>
            public PassthroughView(PageOverlay windowOverlay, CGRect frame)
                : base(frame)
            {
                overlay = windowOverlay;
            }

            public override bool PointInside(CGPoint point, UIEvent? uievent)
            {
                foreach(var element in this.overlay.elements)
                {
                    var boundingBox = element.GetBoundingBox();
                    if (boundingBox.Contains(point.X, point.Y))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }

    public static class VisualDiagnosticsOverlayExtensions
    {
        internal static System.Numerics.Matrix4x4 GetViewTransform(this IView view)
        {
            var nativeView = view?.GetNative(true);
            if (nativeView == null)
                return new System.Numerics.Matrix4x4();
            return nativeView.Layer.GetViewTransform();
        }

        internal static System.Numerics.Matrix4x4 GetViewTransform(this UIView view)
            => view.Layer.GetViewTransform();

        internal static Microsoft.Maui.Graphics.Rectangle GetBoundingBox(this IView view)
            => view.GetNative(true).GetBoundingBox();

        internal static Microsoft.Maui.Graphics.Rectangle GetBoundingBox(this UIView? nativeView)
        {
            if (nativeView == null)
                return new Rectangle();
            var nvb = nativeView.GetNativeViewBounds();
            var transform = nativeView.GetViewTransform();
            var radians = transform.ExtractAngleInRadians();
            var rotation = CoreGraphics.CGAffineTransform.MakeRotation((nfloat)radians);
            CGAffineTransform.CGRectApplyAffineTransform(nvb, rotation);
            return new Rectangle(nvb.X, nvb.Y, nvb.Width, nvb.Height);
        }

        internal static double ExtractAngleInRadians(this System.Numerics.Matrix4x4 matrix) => Math.Atan2(matrix.M21, matrix.M11);

        internal static Rectangle GetNativeViewBounds(this IView view)
        {
            var nativeView = view?.GetNative(true);
            if (nativeView == null)
            {
                return new Rectangle();
            }

            return nativeView.GetNativeViewBounds();
        }

        internal static Rectangle GetNativeViewBounds(this UIView nativeView)
        {
            if (nativeView == null)
                return new Rectangle();

            var superview = nativeView;
            while (superview.Superview is not null)
            {
                superview = superview.Superview;
            }

            var convertPoint = nativeView.ConvertRectToView(nativeView.Bounds, superview);

            var X = convertPoint.X;
            var Y = convertPoint.Y;
            var Width = convertPoint.Width;
            var Height = convertPoint.Height;

            return new Rectangle(X, Y, Width, Height);
        }
    }

    internal static class CoreAnimationExtensions
    {
        internal static Matrix4x4 ToViewTransform(this CATransform3D transform) =>
            new Matrix4x4
            {
                M11 = (float)transform.m11,
                M12 = (float)transform.m12,
                M13 = (float)transform.m13,
                M14 = (float)transform.m14,
                M21 = (float)transform.m21,
                M22 = (float)transform.m22,
                M23 = (float)transform.m23,
                M24 = (float)transform.m24,
                M31 = (float)transform.m31,
                M32 = (float)transform.m32,
                M33 = (float)transform.m33,
                M34 = (float)transform.m34,
                Translation = new Vector3((float)transform.m41, (float)transform.m42, (float)transform.m43),
                M44 = (float)transform.m44
            };

        internal static Matrix4x4 GetViewTransform(this CALayer layer)
        {
            if (layer == null)
                return new Matrix4x4();

            var superLayer = layer.SuperLayer;
            if (layer.Transform.IsIdentity && (superLayer == null || superLayer.Transform.IsIdentity))
                return new Matrix4x4();

            var superTransform = layer.SuperLayer?.GetChildTransform() ?? CATransform3D.Identity;

            return layer.GetLocalTransform()
                .Concat(superTransform)
                    .ToViewTransform();
        }

        internal static CATransform3D Prepend(this CATransform3D a, CATransform3D b) =>
            b.Concat(a);

        internal static CATransform3D GetLocalTransform(this CALayer layer)
        {
            return CATransform3D.Identity
                .Translate(
                    layer.Position.X,
                    layer.Position.Y,
                    layer.ZPosition)
                .Prepend(layer.Transform)
                .Translate(
                    -layer.AnchorPoint.X * layer.Bounds.Width,
                    -layer.AnchorPoint.Y * layer.Bounds.Height,
                    -layer.AnchorPointZ);
        }

        internal static CATransform3D GetChildTransform(this CALayer layer)
        {
            var childTransform = layer.SublayerTransform;

            if (childTransform.IsIdentity)
                return childTransform;

            return CATransform3D.Identity
                .Translate(
                    layer.AnchorPoint.X * layer.Bounds.Width,
                    layer.AnchorPoint.Y * layer.Bounds.Height,
                    layer.AnchorPointZ)
                .Prepend(childTransform)
                .Translate(
                    -layer.AnchorPoint.X * layer.Bounds.Width,
                    -layer.AnchorPoint.Y * layer.Bounds.Height,
                    -layer.AnchorPointZ);
        }

        internal static CATransform3D TransformToAncestor(this CALayer fromLayer, CALayer toLayer)
        {
            var transform = CATransform3D.Identity;

            CALayer? current = fromLayer;
            while (current != toLayer)
            {
                transform = transform.Concat(current.GetLocalTransform());

                current = current.SuperLayer;
                if (current == null)
                    break;

                transform = transform.Concat(current.GetChildTransform());
            }
            return transform;
        }
    }
}
