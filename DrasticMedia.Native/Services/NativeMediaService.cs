// <copyright file="NativeMediaService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;

namespace DrasticMedia.Core.Services
{
    /// <summary>
    /// Native Media Service.
    /// </summary>
    public partial class NativeMediaService : IMediaService
    {
        private System.Threading.Timer positionTimer;

        private Model.MediaItem? media;

        /// <inheritdoc/>
        public event EventHandler<MediaPlayerPositionChangedEventArgs>? PositionChanged;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? EndCurrentItemReached;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? RaiseCanExecuteChanged;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? MediaChanged;

        /// <inheritdoc/>
        public Model.MediaItem? CurrentMedia { get { return this.media; } set { this.media = value; this.SetCurrentMedia(); } }

        internal void PositionTimerElapsed(object? state)
        {
            this.PositionChanged?.Invoke(this, new MediaPlayerPositionChangedEventArgs(this.CurrentPosition));
        }

        private void SetCurrentMedia()
        {
            var path = this.CurrentMedia?.Path ?? this.CurrentMedia?.OnlinePath?.ToString();

            if (path == null)
            {
                throw new NullReferenceException(nameof(this.CurrentMedia));
            }

            this.SetCurrentMediaNative();
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
        }

#if !ANDROID && !IOS && !MACCATALYST && !WINDOWS

        internal void SetCurrentMediaNative()
        {

        }

        public Task<string> GetArtworkUrl()
        {
            throw new NotImplementedException();
        }

        public Task PauseAsync()
        {
            throw new NotImplementedException();
        }

        public Task PlayAsync(double position = 0, bool fromPosition = false)
        {
            throw new NotImplementedException();
        }

        public Task ResumeAsync()
        {
            throw new NotImplementedException();
        }

        public Task SkipAhead(double amount = 0)
        {
            throw new NotImplementedException();
        }

        public Task SkipBack(double amount = 0)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync()
        {
            throw new NotImplementedException();
        }

        public bool IsPlaying => throw new NotImplementedException();

        public float CurrentPosition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
#endif
    }
}
