// <copyright file="MediaLibraryTests.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DrasticMedia.Core.Database;
using DrasticMedia.Core.Helpers;
using DrasticMedia.Core.Library;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Platform;
using DrasticMedia.Core.Services;
using DrasticMedia.SQLite.Database;
using LibVLCSharp.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DrasticMedia.Core.Tests
{
    /// <summary>
    /// Media Library Tests.
    /// </summary>
    [TestClass]
    public class MediaLibraryTests
    {
        private IPodcastService podcastService;
        private LibVLC libVLC;
        private ILogger logger;
        private IPodcastDatabase podcastDB;
        private IVideoDatabase videoDB;
        private IMusicDatabase musicDB;
        private IPlatformSettings settings;
        private MediaLibrary mediaLibrary;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaLibraryTests"/> class.
        /// </summary>
        public MediaLibraryTests()
        {
            this.settings = new MockPlatformSettings();
            this.libVLC = new LibVLC();
            this.logger = new ConsoleLogger();
            this.podcastService = new PodcastService(this.logger);
            this.podcastDB = new PodcastDatabase(ExtensionHelpers.PodcastDatabase());
            this.videoDB = new VideoDatabase(ExtensionHelpers.VideoDatabase());
            this.musicDB = new MusicDatabase(ExtensionHelpers.MusicDatabase());
            this.mediaLibrary = new MediaLibrary(this.libVLC, this.musicDB, this.videoDB, this.podcastDB, this.settings, this.logger);
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
                Assert.IsNotNull(ep.EpisodeUri);
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
            var podcastItem = await this.mediaLibrary.AddOrUpdatePodcastFromUri(new System.Uri(feeduri));
            Assert.IsNotNull(podcastItem);
            Assert.IsTrue(podcastItem.Id > 0);
            foreach (var episode in podcastItem.Episodes)
            {
                Assert.IsTrue(episode.Id > 0);
            }

            var podcasts = await this.mediaLibrary.FetchPodcastsAsync();
            Assert.IsTrue(podcasts.Any());

            var podcast = await this.mediaLibrary.FetchPodcastWithEpisodesAsync(podcasts.First().Id);
            Assert.IsNotNull(podcast);
            Assert.IsTrue(podcast.Episodes.Any());

            await this.mediaLibrary.RemovePodcast(podcast);

            var oldPodcast = await this.mediaLibrary.FetchPodcastWithEpisodesAsync(podcast.Id);
            Assert.IsNull(oldPodcast);
        }
    }
}
