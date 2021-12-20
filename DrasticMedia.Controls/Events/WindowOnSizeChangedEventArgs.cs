// <copyright file="WindowOnSizeChangedEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticMedia.Core.Events
{
    /// <summary>
    /// WindowOnSizeChangedEventArgs.
    /// </summary>
    public class WindowOnSizeChangedEventArgs : EventArgs
    {

        /// <summary>
        /// Width.
        /// </summary>
        public readonly double Width;

        /// <summary>
        /// Width.
        /// </summary>
        public readonly double Height;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowOnSizeChangedEventArgs"/> class.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">height.</param>
        internal WindowOnSizeChangedEventArgs(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
}
