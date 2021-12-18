// <copyright file="DragAndDropOverlay.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;
using LibVLCSharp.Shared;

namespace DrasticMedia.Overlays
{
    /// <summary>
    /// Drag and drop overlay.
    /// </summary>
    public partial class DragAndDropOverlay : WindowOverlay
    {
        private DropElementOverlay dropElement;
        private bool dragAndDropOverlayNativeElementsInitialized;

        internal LibVLC libVLC;

        internal bool IsDragging
        {
            get => dropElement.IsDragging;
            set
            {
                dropElement.IsDragging = value;
                this.Invalidate();
            }
        }

        public DragAndDropOverlay(IWindow window, LibVLC libVLC)
            : base(window)
        {
            this.libVLC = libVLC;
            this.dropElement = new DropElementOverlay();
            this.AddWindowElement(dropElement);
        }

        public event EventHandler<DragAndDropOverlayTappedEventArgs>? Drop;

        public event EventHandler? SizeChanged;

        class DropElementOverlay : IWindowOverlayElement
        {
            public bool IsDragging { get; set; }

            // We are not going to use Contains for this.
            // We're gonna set if it's invoked externally.
            public bool Contains(Point point) => false;

            public void Draw(ICanvas canvas, RectangleF dirtyRect)
            {
                if (!this.IsDragging)
                    return;

                // We're going to fill the screen with a transparent
                // color to show the drag and drop is happening.
                canvas.FillColor = Color.FromRgba(225, 0, 0, 100);
                canvas.FillRectangle(dirtyRect);
            }
        }
    }

    /// <summary>
    /// Drag and Drop Overlay Tapped Event Args.
    /// </summary>
    public class DragAndDropOverlayTappedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DragAndDropOverlayTappedEventArgs"/> class.
        /// </summary>
        /// <param name="files">Media Items.</param>
        public DragAndDropOverlayTappedEventArgs(IList<MediaItem> files)
        {
            this.Files = files;
        }

        public IList<MediaItem> Files { get; }
    }
}
