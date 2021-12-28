// <copyright file="LastfmMetadataTests.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using DrasticMedia.Core.Metadata;
using DrasticMedia.Core.Model;
using DrasticMedia.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DrasticMedia.Metadata.Test;

[TestClass]
public class SpotifyMetadataTests
{
    private IMetadataService? spotifyMetadataService;

    public SpotifyMetadataTests()
    {
    }

    [TestMethod]
    public async Task UpdateArtistInfo()
    {
        //var artist = new ArtistItem() { Name = "Bad Religion" };
        //await this.spotifyMetadataService.UpdatetArtistItemInfo(artist);
    }
}