// <copyright file="DatabaseTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Audio.Library;
using DrasticMedia.Core;
using DrasticMedia.Core.Database;
using DrasticMedia.Core.Library;
using DrasticMedia.Core.Platform;
using DrasticMedia.Core.Services;
using DrasticMedia.Podcast.Library;
using DrasticMedia.SQLite.Database;
using DrasticMedia.SQLite.Native.Test;
using DrasticMedia.Tests;
using DrasticMedia.Video.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace DrasticMedia.SQLite.Test;

[TestClass]
public class DatabaseTest
{
    private ILogger logger;
    private IPodcastDatabase podcastDB;
    private IVideoDatabase videoDB;
    private IMusicDatabase musicDB;
    private IPlatformSettings settings;
    private ILocalMetadataParser localMetadataParser;
    private IPodcastLibrary podcastLibrary;
    private IVideoLibrary videoLibrary;
    private IAudioLibrary audioLibrary;
    private IPodcastService podcastService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseTest"/> class.
    /// </summary>
    public DatabaseTest()
    {
        this.settings = new MockPlatformSettings();
        this.localMetadataParser = new FFMpegMediaParser(ExtensionHelpers.MetadataLocation());
        this.logger = new ConsoleLogger();
        this.podcastService = new PodcastService(this.logger);
        this.podcastDB = new PodcastDatabase(ExtensionHelpers.PodcastDatabase());
        this.videoDB = new VideoDatabase(ExtensionHelpers.VideoDatabase());
        this.musicDB = new MusicDatabase(ExtensionHelpers.MusicDatabase());
        this.podcastLibrary = new PodcastLibrary(this.podcastService, this.podcastDB);
        this.audioLibrary = new AudioLibrary(this.localMetadataParser, this.musicDB, this.settings, null, this.logger);
        this.videoLibrary = new VideoLibrary(this.localMetadataParser, this.videoDB, this.settings, this.logger);
    }

    /// <summary>
    /// Can parse Podcast Feeds.
    /// </summary>
    /// <param name="feeduri">The feed uri.</param>
    [DataRow(@"https://feeds.fireside.fm/xamarinpodcast/rss")]
    [DataRow(@"http://feeds.codenewbie.org/cnpodcast.xml")]
    [DataRow(@"https://feeds.fireside.fm/mergeconflict/rss")]
    [DataRow(@"https://msdevshow.libsyn.com/rss")]
    [DataRow(@"https://thedotnetcorepodcast.libsyn.com/rss")]
    [DataRow(@"https://devchat.tv/podcasts/adventures-in-dotnet/feed/")]
    [DataRow(@"https://feeds.simplecast.com/gvtxUiIf")]
    [DataRow(@"https://microsoftmechanics.libsyn.com/rss")]
    [DataRow(@"https://feeds.simplecast.com/cRTTfxcT")]
    [DataRow(@"https://intrazone.libsyn.com/rss")]
    [DataRow(@"https://upwards.libsyn.com/rss")]
    [DataRow(@"https://listenbox.app/f/NRGnlt0wQqB7")]
    [DataRow(@"https://feeds.buzzsprout.com/978640.rss")]
    [DataRow(@"https://feeds.soundcloud.com/users/soundcloud:users:941029057/sounds.rss")]
    [DataRow(@"https://feeds.fireside.fm/xamarinpodcast/rss")]
    [DataTestMethod]
    public async Task ParsePodcastInfo(string feeduri)
    {
        var result = await this.podcastService.FetchPodcastShowAsync(new System.Uri(feeduri), System.Threading.CancellationToken.None);
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Image);
        Assert.IsNotNull(result.Title);
        Assert.IsNotNull(result.Author);
        Assert.IsNotNull(result.Copyright);
        Assert.IsNotNull(result.PodcastFeed);
        Assert.IsNotNull(result.SiteUri);
        Assert.IsNotNull(result.Description);
        Assert.IsNotNull(result.Episodes);

        foreach (var ep in result.Episodes)
        {
            Assert.IsNotNull(ep.Title);
            Assert.IsNotNull(ep.OnlinePath);
            Assert.IsNotNull(ep.Description);
            Assert.IsNotNull(ep.ReleaseDate);
            Assert.IsTrue(ep.Duration != default);
            Assert.IsTrue(ep.Duration.TotalSeconds > 0);
        }
    }

    /// <summary>
    /// Can parse Podcast Feeds.
    /// </summary>
    /// <param name="feeduri">The feed uri.</param>
    [DataRow(@"https://feeds.fireside.fm/mergeconflict/rss")]
    [DataTestMethod]
    public async Task AddUpdateRemovePodcast(string feeduri)
    {
        var podcastItem = await this.podcastLibrary.AddOrUpdatePodcastFromUri(new System.Uri(feeduri));
        Assert.IsNotNull(podcastItem);
        Assert.IsTrue(podcastItem.Id > 0);
        foreach (var episode in podcastItem.Episodes)
        {
            Assert.IsTrue(episode.Id > 0);
        }

        var podcasts = await this.podcastLibrary.FetchPodcastsAsync();
        Assert.IsTrue(podcasts.Any());

        var podcast = await this.podcastLibrary.FetchPodcastWithEpisodesAsync(podcasts.First().Id);
        Assert.IsNotNull(podcast);
        Assert.IsTrue(podcast.Episodes.Any());

        await this.podcastLibrary.RemovePodcast(podcast);

        var oldPodcast = await this.podcastLibrary.FetchPodcastWithEpisodesAsync(podcast.Id);
        Assert.IsNull(oldPodcast);
    }
}