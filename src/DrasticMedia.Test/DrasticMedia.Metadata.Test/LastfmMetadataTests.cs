// <copyright file="LastfmMetadataTests.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using DrasticMedia.Core.Metadata;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Model.Metadata;
using DrasticMedia.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DrasticMedia.Metadata.Test;

[TestClass]
public class LastfmMetadataTests
{
    private IAudioMetadataService lastfmService;

    public LastfmMetadataTests()
    {
        this.lastfmService = new LastfmMetadataService(ExtensionHelpers.MetadataLocation(), DrasticMedia.Core.Tools.ApiTokens.LastFMClientToken, DrasticMedia.Core.Tools.ApiTokens.LastFMClientSecretToken);
    }

    [DataRow(@"Bad Religion")]
    [DataTestMethod]
    public async Task GetArtistMetadata(string artistName)
    {
        var artist = new ArtistItem() { Id = 1, Name = artistName };
        var metadata = await this.lastfmService.GetArtistMetadataAsync(artist);
        Assert.IsNotNull(metadata);

        var lastfmMetadata = (ArtistLastFmMetadata)metadata;
        Assert.IsNotNull(lastfmMetadata);

        Assert.IsNotNull(lastfmMetadata.Biography);
        Assert.IsNotNull(lastfmMetadata.Name);
        Assert.IsTrue(lastfmMetadata.ArtistItemId > 0);
    }

    [DataRow(@"Bad Religion", "Against The Grain")]
    [DataTestMethod]
    public async Task GetAlbumtMetadata(string artistName, string albumName)
    {
        var artist = new ArtistItem() { Id = 1, Name = artistName };
        var album = new AlbumItem() { Id = 1, Name = albumName, ArtistItem = artist };

        var metadata = await this.lastfmService.GetAlbumMetadataAsync(album);
        Assert.IsNotNull(metadata);

        var lastfmMetadata = (AlbumLastFmMetadata)metadata;
        Assert.IsNotNull(lastfmMetadata);

        Assert.IsNotNull(lastfmMetadata.Name);
        Assert.IsTrue(lastfmMetadata.AlbumItemId > 0);
    }
}