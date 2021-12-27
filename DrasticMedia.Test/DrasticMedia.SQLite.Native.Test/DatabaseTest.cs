// <copyright file="DatabaseTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DrasticMedia.Core;
using DrasticMedia.Core.Database;
using DrasticMedia.Core.Library;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Platform;
using DrasticMedia.Core.Services;
using DrasticMedia.SQLite.Database;
using DrasticMedia.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DrasticMedia.SQLite.Native.Test;

/// <summary>
/// Database Test.
/// </summary>
[TestClass]
public class DatabaseTest
{
    private ILogger logger;
    private IPodcastDatabase podcastDB;
    private IVideoDatabase videoDB;
    private IMusicDatabase musicDB;
    private IPlatformSettings settings;
    private ILocalMetadataParser localMetadataParser;
    private MediaLibrary mediaLibrary;
    private IPodcastService podcastService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseTest"/> class.
    /// </summary>
    public DatabaseTest()
    {
        this.settings = new MockPlatformSettings();
        this.localMetadataParser = new NativeMediaParser(ExtensionHelpers.MetadataLocation());
        this.logger = new ConsoleLogger();
        this.podcastService = new PodcastService(this.logger);
        this.podcastDB = new PodcastDatabase(ExtensionHelpers.PodcastDatabase());
        this.videoDB = new VideoDatabase(ExtensionHelpers.VideoDatabase());
        this.musicDB = new MusicDatabase(ExtensionHelpers.MusicDatabase());
        this.mediaLibrary = new MediaLibrary(this.localMetadataParser, this.musicDB, this.videoDB, this.podcastDB, this.settings, this.logger);
    }

    /// <summary>
    /// Can the databases initialize.
    /// </summary>
    [TestMethod]
    public void DatabasesCanInitialize()
    {
        Assert.IsTrue(this.podcastDB.IsInitialized);
        Assert.IsTrue(this.musicDB.IsInitialized);
        Assert.IsTrue(this.videoDB.IsInitialized);
    }

    /// <summary>
    /// Parse Media Directory.
    /// </summary>
    /// <param name="mediaDirectory">Media Directory.</param>
    /// <param name="type">Media Type.</param>
    /// <returns>Task.</returns>
    [DataRow(@"Media/Media Library Test/Music", Core.Library.MediaType.Audio)]
    [DataRow(@"Media/Media Library Test/Video", Core.Library.MediaType.Video)]
    [DataTestMethod]
    public async Task ParseMediaDiriectory(string mediaDirectory, Core.Library.MediaType type)
    {
        this.AddEventHandlers();

        // Normally, we would throw this on a background thread.
        // For this test, we're going to await for the result.
        await this.mediaLibrary.ScanMediaDirectoriesAsync(mediaDirectory);

        if (type == Core.Library.MediaType.Audio)
        {
            var artists = await this.mediaLibrary.FetchArtistsAsync();
            Assert.IsTrue(artists.Any());

            var artistNoAlbum = await this.mediaLibrary.FetchArtistViaIdAsync(artists[0].Id);
            Assert.IsNotNull(artistNoAlbum);

            var artistWithAlbum = await this.mediaLibrary.FetchArtistWithAlbumsViaIdAsync(artists[0].Id);
            Assert.IsNotNull(artistWithAlbum);
            Assert.IsTrue(artistWithAlbum.Albums.Any());

            var albumNoTracks = await this.mediaLibrary.FetchAlbumViaIdAsync(artistWithAlbum.Albums[0].Id);
            Assert.IsNotNull(albumNoTracks);

            var albumWithTracks = await this.mediaLibrary.FetchAlbumWithTracksViaIdAsync(artists[0].Id);
            Assert.IsNotNull(albumWithTracks);
            Assert.IsTrue(albumWithTracks.Tracks.Any());
        }

        if (type == Core.Library.MediaType.Video)
        {
            var videos = await this.mediaLibrary.FetchVideosAsync();
            Assert.IsTrue(videos.Any());

            // TODO: Test TV Shows.
            //var tvShows = await this.mediaLibrary.FetchTVShowsAsync();
            //Assert.IsTrue(tvShows.Any());

            //var tvShow = await this.mediaLibrary.FetchTVShowWithEpisodesAsync(tvShows[0].Id);
            //Assert.IsNotNull(tvShow);
            //Assert.IsTrue(tvShow.Episodes.Any());
        }

        this.RemoveEventHandlers();
    }

    private void MediaLibrary_UpdateMediaItemAdded(object? sender, UpdateMediaItemEventArgs e)
    {
        if (e != null)
        {
            this.logger.Log(Core.LogLevel.Info, e.ToString());
        }
    }

    private void MediaLibrary_RemoveMediaItem(object? sender, RemoveMediaItemEventArgs e)
    {
        if (e != null)
        {
            this.logger.Log(Core.LogLevel.Info, e.ToString());
        }
    }

    private void MediaLibrary_NewMediaItemError(object? sender, NewMediaItemErrorEventArgs e)
    {
        if (e != null)
        {
            this.logger.Log(Core.LogLevel.Info, e.ToString());
        }
    }

    private void MediaLibrary_NewMediaItemAdded(object? sender, NewMediaItemEventArgs e)
    {
        if (e != null)
        {
            this.logger.Log(Core.LogLevel.Info, e.ToString());
        }
    }

    private void AddEventHandlers()
    {
        this.mediaLibrary.NewMediaItemAdded += this.MediaLibrary_NewMediaItemAdded;
        this.mediaLibrary.NewMediaItemError += this.MediaLibrary_NewMediaItemError;
        this.mediaLibrary.RemoveMediaItem += this.MediaLibrary_RemoveMediaItem;
        this.mediaLibrary.UpdateMediaItemAdded += this.MediaLibrary_UpdateMediaItemAdded;
    }

    private void RemoveEventHandlers()
    {
        this.mediaLibrary.NewMediaItemAdded -= this.MediaLibrary_NewMediaItemAdded;
        this.mediaLibrary.NewMediaItemError -= this.MediaLibrary_NewMediaItemError;
        this.mediaLibrary.RemoveMediaItem -= this.MediaLibrary_RemoveMediaItem;
        this.mediaLibrary.UpdateMediaItemAdded -= this.MediaLibrary_UpdateMediaItemAdded;
    }
}