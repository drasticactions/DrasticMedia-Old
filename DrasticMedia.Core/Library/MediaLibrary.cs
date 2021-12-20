// <copyright file="MediaLibrary.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Database;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Platform;
using DrasticMedia.Core.Services;

namespace DrasticMedia.Core.Library
{
    /// <summary>
    /// Media Library.
    /// </summary>
    public class MediaLibrary : IMediaLibrary
    {
        private ILogger logger;
        private IMusicDatabase musicDatabase;
        private IVideoDatabase videoDatabase;
        private IPodcastDatabase podcastDatabase;
        private IPlatformSettings platform;
        private IPodcastService podcastService;
        private ILocalMetadataParser mediaParser;
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaLibrary"/> class.
        /// </summary>
        /// <param name="mediaParser">mediaParser Instance.</param>
        /// <param name="musicDatabase">Music Database.</param>
        /// <param name="videoDatabase">Video Database.</param>
        /// <param name="podcastDatabase">Podcast Database.</param>
        /// <param name="platform">Storage File APIs.</param>
        /// <param name="logger">Logger.</param>
        public MediaLibrary(ILocalMetadataParser mediaParser, IMusicDatabase musicDatabase, IVideoDatabase videoDatabase, IPodcastDatabase podcastDatabase, IPlatformSettings platform, ILogger logger)
        {
            this.platform = platform;
            this.logger = logger;
            this.mediaParser = mediaParser;
            this.podcastDatabase = podcastDatabase;
            this.musicDatabase = musicDatabase;
            this.videoDatabase = videoDatabase;
            this.podcastService = new PodcastService(this.logger);
            if (!this.musicDatabase.IsInitialized || !this.videoDatabase.IsInitialized || !this.podcastDatabase.IsInitialized)
            {
                throw new ArgumentException($"Databases must be initialized before using them in the media library.");
            }
        }

        /// <inheritdoc/>
        public event EventHandler<NewMediaItemEventArgs>? NewMediaItemAdded;

        /// <inheritdoc/>
        public event EventHandler<UpdateMediaItemEventArgs>? UpdateMediaItemAdded;

        /// <inheritdoc/>
        public event EventHandler<RemoveMediaItemEventArgs>? RemoveMediaItem;

        /// <inheritdoc/>
        public event EventHandler<NewMediaItemErrorEventArgs>? NewMediaItemError;

        /// <inheritdoc/>
        public Task<List<VideoItem>> FetchVideosAsync() => this.videoDatabase.FetchVideosAsync();

        /// <inheritdoc/>
        public Task<List<TVShow>> FetchTVShowsAsync() => this.videoDatabase.FetchTVShowsAsync();

        /// <inheritdoc/>
        public Task<TVShow?> FetchTVShowWithEpisodesAsync(int id) => this.videoDatabase.FetchTVShowWithEpisodesAsync(id);

        /// <inheritdoc/>
        public Task<TVShow?> FetchTVShowViaNameAsync(string name) => this.videoDatabase.FetchTVShowViaNameAsync(name);

        /// <inheritdoc/>
        public async Task ScanMediaDirectoriesAsync(string mediaDirectory)
        {
            this.logger.Log(LogLevel.Debug, $"ScanMediaDirectories: {mediaDirectory}");
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
                var result = await this.AddFileAsync(file);
                this.logger.Log(LogLevel.Debug, $"ScanMediaDirectory: {file} - {result}");
            }
        }

        /// <inheritdoc/>
        public async Task<AlbumItem?> FetchAlbumWithTracksViaIdAsync(int id) => await this.musicDatabase.FetchAlbumWithTracksViaIdAsync(id);

        /// <inheritdoc/>
        public async Task<List<ArtistItem>> FetchArtistsAsync() => await this.musicDatabase.FetchArtistsAsync();

        /// <inheritdoc/>
        public async Task<ArtistItem?> FetchArtistViaIdAsync(int id) => await this.musicDatabase.FetchArtistViaIdAsync(id);

        /// <inheritdoc/>
        public async Task<ArtistItem?> FetchArtistWithAlbumsViaIdAsync(int id) => await this.musicDatabase.FetchArtistWithAlbumsViaIdAsync(id);

        /// <inheritdoc/>
        public async Task<ArtistItem?> FetchArtistViaNameAsync(string name) => await this.musicDatabase.FetchArtistViaNameAsync(name);

        /// <inheritdoc/>
        public async Task<List<AlbumItem>> FetchAlbumsAsync() => await this.musicDatabase.FetchAlbumsAsync();

        /// <inheritdoc/>
        public async Task<AlbumItem?> FetchAlbumViaIdAsync(int id) => await this.musicDatabase.FetchAlbumViaIdAsync(id);

        /// <inheritdoc/>
        public async Task<TrackItem?> FetchTrackViaIdAsync(int id) => await this.musicDatabase.FetchTrackViaIdAsync(id);

        /// <inheritdoc/>
        public async Task<List<TrackItem>> FetchTracksAsync() => await this.musicDatabase.FetchTracksAsync();

        /// <inheritdoc/>
        public async Task RemoveTVShowAsync(TVShow show)
        {
            show = await this.videoDatabase.RemoveTVShowAsync(show).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(show));
        }

        /// <inheritdoc/>
        public async Task RemoveVideoItem(VideoItem videoItem)
        {
            videoItem = await this.videoDatabase.RemoveVideoItemAsync(videoItem).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(videoItem));
            if (videoItem.TvShow is not null && videoItem.TvShow.Episodes.Count <= 0)
            {
                await this.RemoveTVShowAsync(videoItem.TvShow).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task RemovePodcast(PodcastShowItem podcast)
        {
            podcast = await this.podcastDatabase.RemovePodcastAsync(podcast).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(podcast));
        }

        /// <inheritdoc/>
        public async Task RemovePodcastEpisode(PodcastEpisodeItem podcast)
        {
            podcast = await this.podcastDatabase.RemoveEpisodeAsync(podcast).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(podcast));
        }

        /// <inheritdoc/>
        public async Task RemoveArtistAsync(ArtistItem artist)
        {
            artist = await this.musicDatabase.RemoveArtistAsync(artist).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(artist));
        }

        /// <inheritdoc/>
        public async Task RemoveAlbumAsync(AlbumItem album)
        {
            album = await this.musicDatabase.RemoveAlbumAsync(album).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(album));
            if (album.ArtistItem?.Albums != null && album.ArtistItem.Albums.Count <= 0)
            {
                await this.RemoveArtistAsync(album.ArtistItem).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task RemoveTrackAsync(TrackItem track)
        {
            track = await this.musicDatabase.RemoveTrackAsync(track).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(track));
            if (track.AlbumItem?.Tracks != null && track.AlbumItem.Tracks.Count <= 0)
            {
                await this.RemoveAlbumAsync(track.AlbumItem).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task<List<PodcastShowItem>> FetchPodcastsAsync() => await this.podcastDatabase.FetchShowsAsync().ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task<PodcastShowItem?> FetchPodcastWithEpisodesAsync(int showId) => await this.podcastDatabase.FetchShowWithEpisodesAsync(showId).ConfigureAwait(false);

        /// <inheritdoc/>
        public async Task<bool> AddFileAsync(string path)
        {
            try
            {
                if (!this.platform.IsFileAvailable(path))
                {
                    return false;
                }

                var fileType = Path.GetExtension(path);
                if (FileExtensions.AudioExtensions.Contains(fileType))
                {
                    var trackInDb = await this.musicDatabase.ContainsTrackAsync(path).ConfigureAwait(false);
                    if (trackInDb)
                    {
                        // Already in DB, return.
                        return true;
                    }

                    var mP = await this.mediaParser.GetMusicPropertiesAsync(path) as TrackItem;
                    if (mP is null || (string.IsNullOrEmpty(mP.Artist) && string.IsNullOrEmpty(mP.Album) && string.IsNullOrEmpty(mP.Title)))
                    {
                        // We couldn't parse the file or the metadata is empty. Skip it.
                        this.OnNewMediaItemError(new NewMediaItemErrorEventArgs() { MediaItemPath = path });
                        return false;
                    }

                    var artistName = mP.Artist?.Trim();
                    if (artistName == null)
                    {
                        return false;
                    }

                    var artist = await this.musicDatabase.FetchArtistViaNameAsync(artistName).ConfigureAwait(false);
                    if (artist is null)
                    {
                        artist = new ArtistItem()
                        {
                            Name = artistName,
                        };
                        artist = await this.musicDatabase.AddArtistAsync(artist);
                        this.OnNewMediaItemAdded(new NewMediaItemEventArgs(artist));
                    }

                    var albumName = mP.Album?.Trim();
                    if (albumName != null)
                    {
                        var album = await this.musicDatabase.FetchAlbumViaNameAsync(artist.Id, albumName);
                        if (album is null)
                        {
                            album = new AlbumItem()
                            {
                                ArtistItemId = artist.Id,
                                Name = albumName,
                                Year = mP.Year,
                            };
                            album = await this.musicDatabase.AddAlbumAsync(album);
                            this.OnNewMediaItemAdded(new NewMediaItemEventArgs(album));
                        }

                        mP.AlbumItemId = album.Id;
                        mP.ArtistItemId = artist.Id;
                    }

                    mP = await this.musicDatabase.AddTrackAsync(mP).ConfigureAwait(false);
                    this.OnNewMediaItemAdded(new NewMediaItemEventArgs(mP));
                    return mP.Id > 0;
                }
                else if (FileExtensions.VideoExtensions.Contains(fileType))
                {
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

                this.OnNewMediaItemError(new NewMediaItemErrorEventArgs() { MediaItemPath = path });
                return false;
            }
            catch (Exception ex)
            {
                this.OnNewMediaItemError(new NewMediaItemErrorEventArgs() { Exception = ex, MediaItemPath = path });
                this.logger.Log(ex);
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task<PodcastShowItem?> AddOrUpdatePodcastFromUri(Uri uri)
        {
            try
            {
                var podcast = await this.podcastDatabase.FetchShowViaUriAsync(uri).ConfigureAwait(false);
                var feed = await this.podcastService.FetchPodcastShowAsync(uri, CancellationToken.None).ConfigureAwait(false);
                if (feed == null)
                {
                    this.NewMediaItemError?.Invoke(this, new NewMediaItemErrorEventArgs() { MediaItemPath = uri.ToString(), MediaType = MediaType.Podcast });
                    return null;
                }

                if (podcast != null)
                {
                    await this.podcastDatabase.UpdatePodcastAsync(podcast);
                    this.OnUpdateMediaItemAdded(new UpdateMediaItemEventArgs(podcast));
                    return podcast;
                }

                await this.podcastDatabase.AddPodcastAsync(feed);
                this.OnNewMediaItemAdded(new NewMediaItemEventArgs(feed));
                return feed;
            }
            catch (Exception ex)
            {
                this.OnNewMediaItemError(new NewMediaItemErrorEventArgs() { Exception = ex, MediaItemPath = uri.ToString(), MediaType = MediaType.Podcast });
                return null;
            }
        }

        /// <inheritdoc/>
        public virtual void OnNewMediaItemAdded(NewMediaItemEventArgs e)
        {
            this.NewMediaItemAdded?.Invoke(this, e);
        }

        /// <inheritdoc/>
        public virtual void OnUpdateMediaItemAdded(UpdateMediaItemEventArgs e)
        {
            this.UpdateMediaItemAdded?.Invoke(this, e);
        }

        /// <inheritdoc/>
        public virtual void OnNewMediaItemError(NewMediaItemErrorEventArgs e)
        {
            this.NewMediaItemError?.Invoke(this, e);
        }

        /// <inheritdoc/>
        public virtual void OnRemoveMediaItem(RemoveMediaItemEventArgs e)
        {
            this.RemoveMediaItem?.Invoke(this, e);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Is Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                }

                this.disposedValue = true;
            }
        }
    }
}
