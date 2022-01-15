// <copyright file="MediaLibrary.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DrasticMedia.Core.Library
{
    /// <summary>
    /// Media Library.
    /// </summary>
    public class MediaLibrary : IMediaLibrary
    {
        private bool disposedValue;

        public MediaLibrary()
        {

        }

        /// <inheritdoc/>
        public event EventHandler<NewMediaItemEventArgs>? NewMediaItemAdded;

        /// <inheritdoc/>
        public event EventHandler<UpdateMediaItemEventArgs>? UpdateMediaItemAdded;

        /// <inheritdoc/>
        public event EventHandler<RemoveMediaItemEventArgs>? RemoveMediaItem;

        /// <inheritdoc/>
        public event EventHandler<NewMediaItemErrorEventArgs>? NewMediaItemError;

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Is Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                }

                this.disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public virtual void OnNewMediaItemAdded(NewMediaItemEventArgs e)
        {
            this.NewMediaItemAdded?.Invoke(this, e);
        }

        /// <inheritdoc/>
        public virtual void OnUpdateMediaItemAdded(UpdateMediaItemEventArgs e)
        {
            this.UpdateMediaItemAdded?.Invoke(this, e);
        }

        /// <inheritdoc/>
        public virtual void OnNewMediaItemError(NewMediaItemErrorEventArgs e)
        {
            this.NewMediaItemError?.Invoke(this, e);
        }

        /// <inheritdoc/>
        public virtual void OnRemoveMediaItem(RemoveMediaItemEventArgs e)
        {
            this.RemoveMediaItem?.Invoke(this, e);
        }

        public virtual async Task<bool> AddFileAsync(string path)
        {
            return false;
        }
    }
}
