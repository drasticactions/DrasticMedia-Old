// <copyright file="IMediaLibrary.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DrasticMedia.Core.Library
{
    /// <summary>
    /// Media Library.
    /// </summary>
    public interface IMediaLibrary : IDisposable
    {
        /// <summary>
        /// Gets the new media item added event.
        /// </summary>
        event EventHandler<NewMediaItemEventArgs>? NewMediaItemAdded;

        /// <summary>
        /// Gets the new media item added event.
        /// </summary>
        event EventHandler<UpdateMediaItemEventArgs>? UpdateMediaItemAdded;

        /// <summary>
        /// Gets the remove media item event.
        /// </summary>
        event EventHandler<RemoveMediaItemEventArgs>? RemoveMediaItem;

        /// <summary>
        /// Gets the new media item error event.
        /// </summary>
        event EventHandler<NewMediaItemErrorEventArgs>? NewMediaItemError;

        /// <summary>
        /// On New Media Item Added.
        /// </summary>
        /// <param name="e">NewMediaItemEventArgs.</param>
        void OnNewMediaItemAdded(NewMediaItemEventArgs e);

        /// <summary>
        /// On Update Media Item Added.
        /// </summary>
        /// <param name="e">UpdateMediaItemEventArgs.</param>
        void OnUpdateMediaItemAdded(UpdateMediaItemEventArgs e);

        /// <summary>
        /// On New Media Item Error.
        /// </summary>
        /// <param name="e">NewMediaItemEventArgs.</param>
        void OnNewMediaItemError(NewMediaItemErrorEventArgs e);

        /// <summary>
        /// On Media Item Removed.
        /// </summary>
        /// <param name="e">RemoveMediaItemEventArgs.</param>
        void OnRemoveMediaItem(RemoveMediaItemEventArgs e);

        /// <summary>
        /// Add file to database async.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <returns>Bool if item was added to the database.</returns>
        Task<bool> AddFileAsync(string path);
    }
}
