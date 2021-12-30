// <copyright file="ParseMediaTest.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.IO;
using System.Threading.Tasks;
using DrasticMedia.Core.Library;
using DrasticMedia.Core.Model;
using DrasticMedia.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DrasticMedia.Native.Test
{
    /// <summary>
    /// Parser Tests.
    /// </summary>
    [TestClass]
    public class ParseMediaTest
    {
        private ILocalMetadataParser mediaParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseMediaTest"/> class.
        /// </summary>
        public ParseMediaTest()
        {
            this.mediaParser = new FFMpegMediaParser(ExtensionHelpers.MetadataLocation());
        }

        /// <summary>
        /// Parse Audio File.
        /// </summary>
        /// <param name="filename">Filename.</param>
        /// <returns>Task.</returns>
        [DataRow(@"Media/Parser Test/test.mp3")]
        [DataTestMethod]
        public async Task ParseAudio(string filename)
        {
            var file = ExtensionHelpers.GetPath(filename);
            Assert.IsTrue(System.IO.File.Exists(file));

            var trackItem = await this.mediaParser.GetMusicPropertiesAsync(file);
            Assert.IsNotNull(trackItem);

            Assert.AreEqual(trackItem.Title, "Test");

            // Album Art should be set on all test tracks.
            Assert.IsNotNull(trackItem.AlbumArt);

            // Album Art should be a file path or URL, not a file url.
            Assert.IsFalse(trackItem.AlbumArt.StartsWith("file://"));

            // We should be able to read the album art file.
            var bytes = File.ReadAllBytes(trackItem.AlbumArt);
            Assert.IsNotNull(bytes);
        }

        /// <summary>
        /// Parse Video File.
        /// </summary>
        /// <param name="filename">Filename.</param>
        /// <returns>Task.</returns>
        [DataRow(@"Media/Parser Test/test.mp4")]
        [DataTestMethod]
        public async Task ParseVideo(string filename)
        {
            var file = ExtensionHelpers.GetPath(filename);
            Assert.IsTrue(System.IO.File.Exists(file));

            var videoItem = await this.mediaParser.GetVideoPropertiesAsync(file);

            Assert.IsNotNull(videoItem);
            Assert.IsFalse(videoItem?.Path?.StartsWith("file://"));

            Assert.IsTrue(File.Exists(videoItem?.Path));

            Assert.IsTrue(videoItem?.Width > 0);
            Assert.IsTrue(videoItem?.Height > 0);
        }
    }
}