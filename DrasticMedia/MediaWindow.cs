﻿// <copyright file="MediaWindow.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMaui.Overlays;
using DrasticMedia.Core.Services;
using DrasticMedia.Services;
using DrasticMedia.Utilities;

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
        private PlayerService player;

        public MediaWindow(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.errorHandler = serviceProvider.GetService<IErrorHandlerService>();
            this.player = serviceProvider.GetService<PlayerService>();
            this.dragAndDropOverlay = new DragAndDropOverlay(this);
            this.dragAndDropOverlay.Drop += DragAndDropOverlay_Drop;
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
            base.OnCreated();
        }

        private void DragAndDropOverlay_Drop(object sender, DragAndDropOverlayTappedEventArgs e)
        {
        }
    }
}