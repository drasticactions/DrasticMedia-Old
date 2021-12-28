// <copyright file="ExtensionHelpers.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticMedia.Tests
{
    public static class ExtensionHelpers
    {
        public static string MetadataLocation() => GetPath("Metadata");

        public static string VideoDatabase() => GetPath("video.test.db");

        public static string MusicDatabase() => GetPath("music.test.db");

        public static string PodcastDatabase() => GetPath("podcast.test.db");

        public static string GetPath(string path)
        {
            var assemblyPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return new DirectoryInfo(System.IO.Path.Join(assemblyPath, path)).FullName;
        }

        public static string? GetSubdirectory(string startingPath, string directoryName)
        {
            if (!System.IO.Directory.Exists(startingPath))
            {
                return null;
            }

            var directories = System.IO.Directory.GetDirectories(startingPath);
            if (directories.Any(n => System.IO.Path.GetFileName(n) == directoryName))
            {
                return new DirectoryInfo(directories.First(n => System.IO.Path.GetFileName(n) == directoryName)).FullName;
            }

            return GetSubdirectory(System.IO.Path.Combine(startingPath, @"..\"), directoryName);
        }
    }
}
