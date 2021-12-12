// <copyright file="Song.cs" company="Drastic Actions">
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
    /// Song.
    /// </summary>
    public class Song : IMedia
    {
        /// <inheritdoc/>
        public int Id { get; set; }

        /// <inheritdoc/>
        public Uri? Location { get; set; }

        /// <inheritdoc/>
        public Uri? Thumbnail { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public long Length { get; set; }

        /// <inheritdoc/>
        public long LastPosition { get; set; }

        /// <inheritdoc/>
        public int TimesPlayed { get; set; }
    }
}
