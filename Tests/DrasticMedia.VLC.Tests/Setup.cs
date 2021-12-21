// <copyright file="Setup.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DrasticMedia.VLC.Tests
{
    /// <summary>
    /// Setup.
    /// </summary>
    [TestClass]
    public class Setup
    {
        /// <summary>
        /// Setup tests when the assembly is loaded.
        /// </summary>
        /// <param name="context">Test Context.</param>
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            MediaSetup();
            DatabaseSetup();
        }

        private static void DatabaseSetup()
        {
            if (File.Exists(ExtensionHelpers.VideoDatabase()))
            {
                File.Delete(ExtensionHelpers.VideoDatabase());
            }

            if (File.Exists(ExtensionHelpers.MusicDatabase()))
            {
                File.Delete(ExtensionHelpers.MusicDatabase());
            }

            if (File.Exists(ExtensionHelpers.PodcastDatabase()))
            {
                File.Delete(ExtensionHelpers.PodcastDatabase());
            }
        }

        private static void MediaSetup()
        {
            LibVLCSharp.Shared.Core.Initialize();

            var mediaPath = ExtensionHelpers.GetPath("Media");
            var mediaTestFiles = ExtensionHelpers.GetPath("MediaTestFiles");
            if (Directory.Exists(mediaPath))
            {
                Directory.Delete(mediaPath, true);
            }

            if (File.Exists(ExtensionHelpers.VideoDatabase()))
            {
                File.Delete(ExtensionHelpers.VideoDatabase());
            }

            if (File.Exists(ExtensionHelpers.MusicDatabase()))
            {
                File.Delete(ExtensionHelpers.MusicDatabase());
            }

            if (File.Exists(ExtensionHelpers.PodcastDatabase()))
            {
                File.Delete(ExtensionHelpers.PodcastDatabase());
            }

            Directory.CreateDirectory(mediaPath);

            string[] files = System.IO.Directory.GetFiles(mediaTestFiles, "*.*", new EnumerationOptions() { RecurseSubdirectories = true });
            foreach (string s in files)
            {
                var destFile = s.Replace("MediaTestFiles", "Media");
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                System.IO.File.Copy(s, destFile, true);
            }
        }
    }
}
