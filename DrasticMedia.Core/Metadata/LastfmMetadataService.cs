// <copyright file="LastfmMetadataService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;
using Hqub.Lastfm;
using Hqub.Lastfm.Entities;

namespace DrasticMedia.Core.Metadata
{
    /// <summary>
    /// Last FM Metadata Service.
    /// </summary>
    public class LastfmMetadataService : IMetadataService
    {
        private LastfmClient client;
        private HttpClient httpClient;

        public LastfmMetadataService(string baseLocation, string apiKey = "", string apiSecret = "")
        {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                return;
            }

            this.client = new LastfmClient(apiKey, apiSecret);
            this.httpClient = new HttpClient();
            if (string.IsNullOrEmpty(baseLocation))
            {
                throw new ArgumentNullException(nameof(baseLocation));
            }

            var directory = Directory.CreateDirectory(baseLocation);
            if (!directory.Exists)
            {
                throw new ArgumentNullException(nameof(baseLocation));
            }

            this.BaseMetadataLocation = baseLocation;
            this.IsEnabled = true;
        }

        /// <inheritdoc/>
        public string BaseMetadataLocation { get; }

        /// <inheritdoc/>
        public bool IsEnabled { get; }

        public async Task UpdatetArtistItemInfo(ArtistItem artist)
        {
            if (!this.IsEnabled)
            {
                return;
            }

            var result = await this.client.Artist.GetInfoAsync(artist.Name);
            if (result is null)
            {
                return;
            }

            artist.Name = result.Name;
            artist.Biography = result.Biography.Content;
            artist.MBID = result.MBID;

            //if (string.IsNullOrEmpty(artist.ArtistImage) && result.Images.Any())
            //{
            //    var image = result.Images[0];
            //    var artPath = System.IO.Path.Combine(this.BaseMetadataLocation, artist.Name,  "artist.jpg");
            //    var imageContent = await this.httpClient.GetByteArrayAsync(image.Url);
            //    if (imageContent != null)
            //    {
            //        System.IO.Directory.CreateDirectory(Path.GetDirectoryName(artPath));
            //        System.IO.File.WriteAllBytes(artPath, imageContent);
            //        if (File.Exists(artPath))
            //        {
            //            artist.ArtistImage = artPath;
            //        }
            //    }
            //}
        }
    }
}
