// <copyright file="MediaPlayerPositionChangedEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DrasticMedia.Core
{
    /// <summary>
    /// The mediaplayer's position changed.
    /// </summary>
    public class MediaPlayerPositionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Mediaplayer's current position.
        /// </summary>
        public readonly float Position;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlayerPositionChangedEventArgs"/> class.
        /// </summary>
        /// <param name="position">Position.</param>
        internal MediaPlayerPositionChangedEventArgs(float position)
        {
            Position = position;
        }
    }
}
