// <copyright file="IMedia.cs" company="Drastic Actions">
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
    /// Media File.
    /// </summary>
    public interface IMedia
    {
        /// <summary>
        /// Gets or sets the id of the media file.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the location of the media.
        /// </summary>
        public Uri? Location { get; set; }

        /// <summary>
        /// Gets or sets the location of the thumbnail for the media file.
        /// </summary>
        public Uri? Thumbnail { get; set; }

        /// <summary>
        /// Gets or sets the name of the media file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the length of the media file.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// Gets or sets the last position of the media file.
        /// </summary>
        public long LastPosition { get; set; }

        /// <summary>
        /// Gets or sets the amount of times the media file has been played.
        /// </summary>
        public int TimesPlayed { get; set; }
    }
}
