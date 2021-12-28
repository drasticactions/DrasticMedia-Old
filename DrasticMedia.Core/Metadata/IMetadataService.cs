// <copyright file="IMetadataService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;

namespace DrasticMedia.Core.Metadata
{
    /// <summary>
    /// Metadata Service.
    /// </summary>
    public interface IMetadataService
    {
        /// <summary>
        /// Gets the base metadata location for where to store parsed files.
        /// </summary>
        string BaseMetadataLocation { get; }

        /// <summary>
        /// Gets a value indicating whether the service is enabled.
        /// </summary>
        public bool IsEnabled { get; }

        /// <summary>
        /// Updates an artist item with updated information.
        /// </summary>
        /// <param name="artist">Artist.</param>
        /// <returns>ArtistItem.</returns>
        Task UpdatetArtistItemInfo(ArtistItem artist);
    }
}
