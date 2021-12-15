// <copyright file="DatabaseTests.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.IO;
using System.Threading.Tasks;
using DrasticMedia.Core.Database;
using DrasticMedia.Core.Helpers;
using DrasticMedia.Core.Library;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Platform;
using DrasticMedia.SQLite.Database;
using LibVLCSharp.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DrasticMedia.Core.Tests
{
    /// <summary>
    /// Database Tests.
    /// </summary>
    [TestClass]
    public class DatabaseTests
    {
        private LibVLC libVLC;
        private ILogger logger;
        private IPodcastDatabase podcastDB;
        private IVideoDatabase videoDB;
        private IMusicDatabase musicDB;
        private IPlatformSettings settings;
        private MediaLibrary mediaLibrary;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseTests"/> class.
        /// </summary>
        public DatabaseTests()
        {
            this.settings = new MockPlatformSettings();
            this.libVLC = new LibVLC();
            this.logger = new ConsoleLogger();
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
    }
}
