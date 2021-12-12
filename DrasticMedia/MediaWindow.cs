// <copyright file="MediaWindow.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Overlays;

namespace DrasticMedia
{
    /// <summary>
    /// Media Window.
    /// </summary>
    public class MediaWindow : Window
    {
        DragAndDropOverlay dragAndDropOverlay;

        public MediaWindow()
        {
            this.dragAndDropOverlay = new DragAndDropOverlay(this);
            this.dragAndDropOverlay.Drop += DragAndDropOverlay_Drop;
        }

        private void DragAndDropOverlay_Drop(object sender, DragAndDropOverlayTappedEventArgs e) => this.Drop?.Invoke(sender, e);

        public event EventHandler<DragAndDropOverlayTappedEventArgs>? Drop;

        protected override void OnCreated()
        {
            this.AddOverlay(this.dragAndDropOverlay);
            base.OnCreated();
        }
    }
}
