// <copyright file="MediaWindow.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Services;
using DrasticMedia.Overlays;
using DrasticMedia.Services;
using DrasticMedia.Utilities;
using LibVLCSharp.Shared;

namespace DrasticMedia
{
    /// <summary>
    /// Media Window.
    /// </summary>
    public class MediaWindow : Window
    {
        private IErrorHandlerService errorHandler;
        private IServiceProvider serviceProvider;
        private DragAndDropOverlay dragAndDropOverlay;
        private PageBackground pageBackgroundOverlay;
        private LibVLC libVLC;
        private PlayerService player;

        public MediaWindow(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.errorHandler = serviceProvider.GetService<IErrorHandlerService>();
            this.player = serviceProvider.GetService<PlayerService>();
            this.libVLC = serviceProvider.GetService<LibVLC>();
            this.dragAndDropOverlay = new DragAndDropOverlay(this, this.libVLC);
            this.pageBackgroundOverlay = new PageBackground(this);
            this.dragAndDropOverlay.Drop += DragAndDropOverlay_Drop;
        }

        /// <inheritdoc/>
        protected override void OnCreated()
        {
            this.AddOverlay(this.dragAndDropOverlay);
            this.AddOverlay(this.pageBackgroundOverlay);
            this.pageBackgroundOverlay.SetPage(new MainPage());
            base.OnCreated();
        }

        private void DragAndDropOverlay_Drop(object sender, DragAndDropOverlayTappedEventArgs e)
        {
            if (e != null && e.Files.Any())
            {
                this.player.AddMedia(e?.Files[0], true).FireAndForgetSafeAsync(this.errorHandler);

                if (e.Files.Count > 1)
                {
                    for (var i = 1; i < e.Files.Count; i++)
                    {
                        this.player.AddMedia(e?.Files[i], false).FireAndForgetSafeAsync(this.errorHandler);
                    }
                }
            }
        }
    }
}
