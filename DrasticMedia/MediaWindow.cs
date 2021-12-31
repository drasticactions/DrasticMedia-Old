﻿// <copyright file="MediaWindow.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMaui.Overlays;
using DrasticMedia.Core.Library;
using DrasticMedia.Core.Services;
using DrasticMedia.Services;
using DrasticMedia.Utilities;
using Microsoft.Maui.Platform;

namespace DrasticMedia
{
    /// <summary>
    /// Media Window.
    /// </summary>
    public class MediaWindow : Window, IVisualTreeElement
    {
        private IErrorHandlerService errorHandler;
        private IServiceProvider serviceProvider;
        private DragAndDropOverlay dragAndDropOverlay;
        private PageOverlay playerOverlay;
        private PlayerService player;
        private MediaLibrary library;
        private PlayerPage playerPage;
        public MediaWindow(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.errorHandler = serviceProvider.GetService<IErrorHandlerService>();
            this.player = serviceProvider.GetService<PlayerService>();
            this.library = serviceProvider.GetService<MediaLibrary>();
            this.dragAndDropOverlay = new DragAndDropOverlay(this);
            this.playerOverlay = new PageOverlay(this);
            this.playerPage = new PlayerPage(this.serviceProvider);
            this.dragAndDropOverlay.Drop += this.DragAndDropOverlay_Drop;
            this.library.NewMediaItemAdded += this.Library_NewMediaItemAdded;
            this.library.NewMediaItemError += this.Library_NewMediaItemError;
            this.library.RemoveMediaItem += this.Library_RemoveMediaItem;
            this.library.UpdateMediaItemAdded += this.Library_UpdateMediaItemAdded;
        }

        /// <inheritdoc/>
        public IReadOnlyList<IVisualTreeElement> GetVisualChildren()
        {
            var elements = new List<IVisualTreeElement>();
            if (this.Page != null && this.Page is IVisualTreeElement element)
            {
                elements.AddRange(element.GetVisualChildren());
            }

            var overlays = this.Overlays.Where(n => n is IVisualTreeElement).Cast<IVisualTreeElement>();
            elements.AddRange(overlays);

            return elements;
        }

        /// <inheritdoc/>
        public IVisualTreeElement? GetVisualParent() => App.Current;

        /// <inheritdoc/>
        protected override void OnCreated()
        {
            this.AddOverlay(this.dragAndDropOverlay);
            this.AddOverlay(this.playerOverlay);
            this.playerOverlay.AddView(this.playerPage);
            base.OnCreated();
        }

        public double GetPlayerHeight() => this.playerPage.GetHeightOfPlayer();

        public double GetHeight()
        {
#if ANDROID
            var nativeView = this.ToNative(this.Handler.MauiContext);
            return (double)nativeView.Height;

#elif WINDOWS
            if (this.Handler.NativeView is Microsoft.UI.Xaml.Window window)
            {
                return window.Bounds.Height;
            }
#endif
            return 0;
        }

        private void DragAndDropOverlay_Drop(object sender, DragAndDropOverlayTappedEventArgs e)
        {
        }

        private void Library_UpdateMediaItemAdded(object sender, UpdateMediaItemEventArgs e)
        {
        }

        private void Library_RemoveMediaItem(object sender, RemoveMediaItemEventArgs e)
        {
        }

        private void Library_NewMediaItemError(object sender, NewMediaItemErrorEventArgs e)
        {
        }

        private void Library_NewMediaItemAdded(object sender, NewMediaItemEventArgs e)
        {
        }
    }
}