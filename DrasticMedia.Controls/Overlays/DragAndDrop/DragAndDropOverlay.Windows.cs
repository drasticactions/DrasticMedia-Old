// <copyright file="DragAndDropOverlay.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core;
using DrasticMedia.Core.Helpers;
using DrasticMedia.Core.Model;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;

namespace DrasticMedia.Overlays
{
    public partial class DragAndDropOverlay
    {
        Microsoft.UI.Xaml.Controls.Panel? panel;

        public override bool Initialize()
        {
            if (dragAndDropOverlayNativeElementsInitialized)
                return true;

            base.Initialize();

            var _nativeElement = Window.Content.GetNative(true);
            if (_nativeElement == null)
                return false;

            var handler = Window.Handler as Microsoft.Maui.Handlers.WindowHandler;
            if (handler?.NativeView is not Microsoft.UI.Xaml.Window _window)
                return false;

            this.panel = _window.Content as Microsoft.UI.Xaml.Controls.Panel;
            if (panel == null)
                return false;

            panel.SizeChanged += Panel_SizeChanged;
            panel.AllowDrop = true;
            panel.DragOver += Panel_DragOver;
            panel.Drop += Panel_Drop;
            panel.DragLeave += Panel_DragLeave;
            panel.DropCompleted += Panel_DropCompleted;
            return dragAndDropOverlayNativeElementsInitialized = true;
        }

        public override bool Deinitialize()
        {
            if (panel != null)
            {
                panel.AllowDrop = false;
                panel.DragOver -= Panel_DragOver;
                panel.Drop -= Panel_Drop;
                panel.DragLeave -= Panel_DragLeave;
                panel.DropCompleted -= Panel_DropCompleted;
                panel.SizeChanged -= Panel_SizeChanged;
            }

            return base.Deinitialize();
        }

        private void Panel_DropCompleted(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.DropCompletedEventArgs args)
        {
            this.IsDragging = false;
        }

        private void Panel_DragLeave(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            this.IsDragging = false;
        }

        private async void Panel_Drop(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Any())
                {
                    var mediaItems = new List<MediaItem>();
                    foreach (var item in items)
                    {
                        if (item is StorageFile storageItem)
                        {
                            var fileType = Path.GetExtension(storageItem.Path);
                            if (FileExtensions.AudioExtensions.Contains(fileType))
                            {
                                var mP = await this.libVLC.GetMusicPropertiesAsync(item.Path) as TrackItem;
                                if (mP != null)
                                {
                                    mediaItems.Add(mP);
                                }
                            }
                            else if (FileExtensions.AudioExtensions.Contains(fileType))
                            {
                                var vP = await this.libVLC.GetVideoPropertiesAsync(item.Path) as VideoItem;
                                if (vP != null)
                                {
                                    mediaItems.Add(vP);
                                }
                            }
                        }
                    }

                    this.Drop?.Invoke(this, new DragAndDropOverlayTappedEventArgs(mediaItems));
                }
            }

            this.IsDragging = false;
        }

        private void Panel_DragOver(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            // For this, we're going to allow "copy"
            // As I want to drag an image into the panel.
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Link;
            this.IsDragging = true;
        }

        private void Panel_SizeChanged(object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
        {
            this.SizeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
