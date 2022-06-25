// <copyright file="Setup.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DrasticMedia.Tests
{
    public class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();

        private NullScope() { }

        public void Dispose() { }
    }

    public class ConsoleLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => false;

        public void Log<TState>(LogLevel logLevel,
                                EventId eventId,
                                TState state,
                                Exception exception,
                                Func<TState, Exception, string> formatter)
        {
            string message = formatter(state, exception);
            Console.WriteLine(message);
        }
    }

    public class ConsoleLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => false;

        public void Log<TState>(LogLevel logLevel,
                                EventId eventId,
                                TState state,
                                Exception exception,
                                Func<TState, Exception, string> formatter)
        {
            string message = formatter(state, exception);
            Console.WriteLine(message);
        }
    }

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

            if (File.Exists(ExtensionHelpers.PodcastDatabaseLog()))
            {
                File.Delete(ExtensionHelpers.PodcastDatabaseLog());
            }
        }

        private static void MediaSetup()
        {
            var mediaPath = ExtensionHelpers.GetPath("Media");
            var mediaTestFiles = ExtensionHelpers.GetSubdirectory(ExtensionHelpers.GetPath(string.Empty), "MediaTestFiles");
            if (Directory.Exists(mediaPath))
            {
                Directory.Delete(mediaPath, true);
            }

            if (File.Exists(ExtensionHelpers.MetadataLocation()))
            {
                File.Delete(ExtensionHelpers.MetadataLocation());
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
                var cutPath = s.Replace($"{mediaTestFiles}{Path.DirectorySeparatorChar}", string.Empty);
                var destFile = Path.Combine(mediaPath, cutPath);
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                System.IO.File.Copy(s, destFile, true);
            }
        }
    }
}
