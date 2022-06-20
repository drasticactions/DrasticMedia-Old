// <copyright file="VideoLibrary.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core;
using DrasticMedia.Core.Database;
using DrasticMedia.Core.Library;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Platform;
using Microsoft.Extensions.Logging;

namespace DrasticMedia.Video.Library
{
    public class VideoLibrary : MediaLibrary, IVideoLibrary, ITVShowLibrary
    {
        private IVideoDatabase videoDatabase;
        private IPlatformSettings platform;
        private ILocalMetadataParser mediaParser;
        private ILogger? logger;

        public VideoLibrary(ILocalMetadataParser mediaParser, IVideoDatabase database, IPlatformSettings platform, ILogger? logger = null)
        {
            this.mediaParser = mediaParser;
            this.videoDatabase = database;
            this.logger = logger;
            this.platform = platform;
        }

        /// <inheritdoc/>
        public Task<List<TVShow>> FetchTVShowsAsync() => this.videoDatabase.FetchTVShowsAsync();

        /// <inheritdoc/>
        public Task<TVShow?> FetchTVShowWithEpisodesAsync(int id) => this.videoDatabase.FetchTVShowWithEpisodesAsync(id);

        /// <inheritdoc/>
        public Task<TVShow?> FetchTVShowViaNameAsync(string name) => this.videoDatabase.FetchTVShowViaNameAsync(name);

        /// <inheritdoc/>
        public async Task RemoveTVShowAsync(TVShow show)
        {
            show = await this.videoDatabase.RemoveTVShowAsync(show).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(show));
        }

        /// <inheritdoc/>
        public Task<List<VideoItem>> FetchVideosAsync() => this.videoDatabase.FetchVideosAsync();

        /// <inheritdoc/>
        public override async Task<bool> AddFileAsync(string path)
        {
            try
            {
                if (!this.platform.IsFileAvailable(path))
                {
                    return false;
                }

                var fileType = Path.GetExtension(path);
                if (!FileExtensions.VideoExtensions.Contains(fileType))
                {
                    return false;
                }

                if (await this.videoDatabase.ContainsVideoAsync(path))
                {
                    return true;
                }

                var videoItem = await this.mediaParser.GetVideoPropertiesAsync(path).ConfigureAwait(false) as VideoItem;
                if (videoItem == null)
                {
                    // We couldn't parse the file or the metadata is empty. Skip it.
                    this.OnNewMediaItemError(new NewMediaItemErrorEventArgs() { MediaItemPath = path });
                    return false;
                }

                if (!string.IsNullOrEmpty(videoItem.ShowTitle))
                {
                    var tvShow = await this.videoDatabase.FetchTVShowViaNameAsync(videoItem.ShowTitle);
                    if (tvShow is null)
                    {
                        tvShow = await this.videoDatabase.AddTVShowAsync(new TVShow() { ShowTitle = videoItem.ShowTitle }).ConfigureAwait(false);
                        this.OnNewMediaItemAdded(new NewMediaItemEventArgs(tvShow));
                    }

                    videoItem.TvShowId = tvShow.Id;
                }

                videoItem = await this.videoDatabase.AddVideoItemAsync(videoItem).ConfigureAwait(false);
                this.OnNewMediaItemAdded(new NewMediaItemEventArgs(videoItem));
                return videoItem.Id > 0;
            }
            catch (Exception ex)
            {
                this.OnNewMediaItemError(new NewMediaItemErrorEventArgs() { Exception = ex, MediaItemPath = path });
                this.logger?.LogError(ex, "OnNewMediaItemError");
            }

            return false;
        }
    }
}
