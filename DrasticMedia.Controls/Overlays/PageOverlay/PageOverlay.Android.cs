﻿// <copyright file="PageOverlay.Android.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Views;
using AndroidX.CoordinatorLayout.Widget;
using Microsoft.Maui.Handlers;

namespace DrasticMedia.Overlays
{
    public partial class PageOverlay
    {
        IMauiContext? mauiContext;
        Activity? _nativeActivity;
        ViewGroup? _nativeLayer;
        Android.Views.View element;

        /// <inheritdoc/>
        public override bool Initialize()
        {
            if (this.pageOverlayNativeElementsInitialized)
            {
                return true;
            }

            if (Window == null)
            {
                return false;
            }

            var nativeWindow = Window?.Content?.GetNative(true);
            if (nativeWindow == null)
            {
                return false;
            }

            var handler = Window?.Handler as WindowHandler;
            if (handler?.MauiContext == null)
            {
                return false;
            }

            this.mauiContext = handler.MauiContext;

            var rootManager = handler.MauiContext.GetNavigationRootManager();
            if (rootManager == null)
            {
                return false;
            }


            if (handler.NativeView is not Activity activity)
            {
                return false;
            }

            _nativeActivity = activity;
            _nativeLayer = rootManager.RootView as ViewGroup;

            if (_nativeLayer?.Context == null)
            {
                return false;
            }

            if (_nativeActivity?.WindowManager?.DefaultDisplay == null)
            {
                return false;
            }

            return this.pageOverlayNativeElementsInitialized = true;
        }

        /// <inheritdoc/>
        public override bool Deinitialize()
        {
            this.RemovePage();
            return base.Deinitialize();
        }

        public void SetPage(Page page, bool toBack = false)
        {
            if (this._nativeLayer == null || this.mauiContext == null)
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
                var layerCount = _nativeLayer.ChildCount;
                var childView = _nativeLayer.GetChildAt(1);
                _nativeLayer.AddView(this.element, layerCount, new CoordinatorLayout.LayoutParams(CoordinatorLayout.LayoutParams.MatchParent, CoordinatorLayout.LayoutParams.MatchParent));
                if (toBack)
                {
                    childView.BringToFront();
                }
                else
                {
                    this.element.BringToFront();
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

            this._nativeLayer?.RemoveView(this.element);
            this.pageSet = false;
            Microsoft.Maui.Controls.Xaml.Diagnostics.VisualDiagnostics.OnChildRemoved(this, this.page, 0);
        }
    }
}