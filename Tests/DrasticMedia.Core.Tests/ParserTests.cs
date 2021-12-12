// <copyright file="ParserTests.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.IO;
using System.Threading.Tasks;
using DrasticMedia.Core.Helpers;
using DrasticMedia.Core.Model;
using LibVLCSharp.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DrasticMedia.Core.Tests
{
    /// <summary>
    /// Parser Tests.
    /// </summary>
    [TestClass]
    public class ParserTests
    {
        private LibVLC libVLC;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserTests"/> class.
        /// </summary>
        public ParserTests()
        {
            this.libVLC = new LibVLC();
        }

        [DataRow(@"Media\Parser Test\test.mp3")]
        [DataRow(@"Media\Parser Test\test.mp4")]
        [DataTestMethod]
        public async Task ParseMedia(string filename)
        {
            var file = ExtensionHelpers.GetPath(filename);
            Assert.IsTrue(System.IO.File.Exists(file));

            var mediaItem = await this.libVLC.GetMediaPropertiesAsync(file);
            Assert.IsNotNull(mediaItem);

            if (mediaItem is TrackItem trackItem)
            {
                Assert.AreEqual(trackItem.Title, "Test");

                // Album Art should be set on all test tracks.
                Assert.IsNotNull(trackItem.AlbumArt);

                // Album Art should be a file path or URL, not a file url.
                Assert.IsFalse(trackItem.AlbumArt.StartsWith("file://"));

                // We should be able to read the album art file.
                var bytes = File.ReadAllBytes(trackItem.AlbumArt);
                Assert.IsNotNull(bytes);
            }

            if (mediaItem is VideoItem videoItem)
            {
                Assert.IsFalse(videoItem.Path.StartsWith("file://"));

                Assert.IsTrue(File.Exists(videoItem.Path));

                Assert.IsTrue(videoItem.Width > 0);
                Assert.IsTrue(videoItem.Height > 0);
            }
        }
    }
}
