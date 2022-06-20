// <copyright file="IMediaActivity.Android.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Services;

namespace DrasticMedia.Native.Activity
{
    /// <summary>
    /// Media Activity.
    /// </summary>
    public interface IMediaActivity
    {
        /// <summary>
        /// Gets or sets the MediaPlayerServiceBinder.
        /// </summary>
        MediaPlayerServiceBinder Binder { get; set; }
    }
}
