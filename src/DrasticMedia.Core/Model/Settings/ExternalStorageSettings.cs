// <copyright file="ExternalStorageSettings.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticMedia.Core.Model
{
    /// <summary>
    /// External Storage Settings.
    /// </summary>
    public enum ExternalStorageSettings
    {
        /// <summary>
        /// Ask me.
        /// </summary>
        AskMe,

        /// <summary>
        /// Use External Storage As Media Library.
        /// </summary>
        UseExternalStorageAsMediaLibrary,

        /// <summary>
        /// Copy Media To Internal Storage.
        /// </summary>
        CopyMediaToInternalStorage,

        /// <summary>
        /// Do Nothing.
        /// </summary>
        DoNothing,
    }
}
