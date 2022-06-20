// <copyright file="MediaScanLibrary.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;

namespace DrasticMedia.Core.Library
{
    public class MediaScanLibrary : IMediaScanLibrary
    {
        private readonly IList<IMediaLibrary> mediaLibraries;
        private ILogger? logger;

        public MediaScanLibrary(IList<IMediaLibrary> mediaLibraries, ILogger? logger = null)
        {
            this.mediaLibraries = mediaLibraries;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task ScanMediaDirectoriesAsync(string mediaDirectory)
        {
            this.logger?.Log(LogLevel.Debug, $"ScanMediaDirectories: {mediaDirectory}");
            await this.ScanMediaDirectoryAsync(mediaDirectory);
            var directories = System.IO.Directory.EnumerateDirectories(mediaDirectory);
            foreach (var directory in directories)
            {
                await this.ScanMediaDirectoriesAsync(directory);
            }
        }

        /// <inheritdoc/>
        public async Task ScanMediaDirectoryAsync(string mediaDirectory)
        {
            var files = Directory.EnumerateFiles(mediaDirectory);
            foreach (var file in files)
            {
                foreach (var library in this.mediaLibraries)
                {
                    var result = await library.AddFileAsync(file);
                    this.logger?.Log(LogLevel.Debug, $"ScanMediaDirectory: {file} - {result}");
                }
            }
        }
    }
}
