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
public class SpotifyMetadataTests
{
    private IMetadataService spotifyMetadataService;

    public SpotifyMetadataTests()
    {
        this.spotifyMetadataService = new SpotifyMetadataService(ExtensionHelpers.MetadataLocation(), Core.Tools.ApiTokens.SpotifyClientToken, Core.Tools.ApiTokens.SpotifyClientSecretToken);
    }

    [DataRow(@"Bad Religion")]
    [DataTestMethod]
    public async Task GetArtistMetadata(string artistName)
    {
        var artist = new ArtistItem() { Id = 1, Name = artistName };
        var metadata = await this.spotifyMetadataService.GetArtistMetadataAsync(artist);
        Assert.IsNotNull(metadata);

        var spotifyMetadata = (ArtistSpotifyMetadata)metadata;
        Assert.IsNotNull(spotifyMetadata);

        Assert.IsNotNull(spotifyMetadata.Image);
        Assert.IsNotNull(spotifyMetadata.Name);
        Assert.IsTrue(spotifyMetadata.ArtistItemId > 0);
    }

    [DataRow(@"Bad Religion", "Against The Grain")]
    [DataTestMethod]
    public async Task GetAlbumtMetadata(string artistName, string albumName)
    {
        var artist = new ArtistItem() { Id = 1, Name = artistName };
        var album = new AlbumItem() { Id = 1, Name = albumName, ArtistItem = artist };

        var metadata = await this.spotifyMetadataService.GetAlbumMetadataAsync(album);
        Assert.IsNotNull(metadata);

        var spotifyMetadata = (AlbumSpotifyMetadata)metadata;
        Assert.IsNotNull(spotifyMetadata);

        Assert.IsNotNull(spotifyMetadata.Image);
        Assert.IsNotNull(spotifyMetadata.Name);
        Assert.IsTrue(spotifyMetadata.AlbumItemId > 0);
    }
}