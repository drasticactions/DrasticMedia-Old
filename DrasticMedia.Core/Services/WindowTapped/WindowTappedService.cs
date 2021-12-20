// <copyright file="WindowTappedService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticMedia.Core.Services
{
    public class WindowTappedService : IWindowTappedService
    {
        private bool screenTapped;
        private bool wasInvoked;
        private const int CursorHiddenAfterSeconds = 4;
        private PeriodicTimer? timer;
        private CancellationTokenSource? cts;
        private readonly IWindow window;
        private readonly IMediaService mediaService;

        public WindowTappedService(IWindow window, IMediaService mediaService)
        {
            this.window = window;
            this.mediaService = mediaService;
            this.mediaService.RaiseCanExecuteChanged += MediaService_RaiseCanExecuteChanged;
            this.window.VisualDiagnosticsOverlay.Tapped += VisualDiagnosticsOverlay_Tapped;
        }

        public event EventHandler OnHidden;

        public event EventHandler OnTapped;

        public void StartService()
        {
            this.cts = new CancellationTokenSource();
            this.timer = new PeriodicTimer(TimeSpan.FromSeconds(CursorHiddenAfterSeconds));
            this.wasInvoked = true;
            Task.Run(this.TimerTask, this.cts.Token);
        }

        public void StopService()
        {
            this.cts?.Cancel();
            this.OnTapped?.Invoke(this, EventArgs.Empty);
        }

        private async Task TimerTask()
        {
            if (this.timer == null || this.cts.IsCancellationRequested)
            {
                return;
            }

            try
            {
                while (await this.timer.WaitForNextTickAsync(this.cts.Token))
                {
                    if (this.screenTapped && this.wasInvoked)
                    {
                        this.screenTapped = false;
                    }
                    else if (!this.screenTapped && this.wasInvoked)
                    {
                        this.OnHidden?.Invoke(this, EventArgs.Empty);
                        this.wasInvoked = false;
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                // Good, we expect this.
            }
        }

        private void VisualDiagnosticsOverlay_Tapped(object? sender, WindowOverlayTappedEventArgs e)
        {
            this.screenTapped = true;
            this.wasInvoked = true;
            this.OnTapped?.Invoke(this, EventArgs.Empty);
        }

        private void MediaService_RaiseCanExecuteChanged(object? sender, EventArgs e)
        {
            if (this.mediaService.IsPlaying)
            {
                this.StartService();
            }
            else
            {
                this.StopService();
            }
        }
    }
}
