// <copyright file="MediaLibrary.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Database;
using DrasticMedia.Core.Helpers;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Platform;
using DrasticMedia.Core.Services;
using LibVLCSharp.Shared;

namespace DrasticMedia.Core.Library
{
    /// <summary>
    /// Media Library.
    /// </summary>
    public class MediaLibrary : IDisposable
    {
        private ILogger logger;
        private IMusicDatabase musicDatabase;
        private IVideoDatabase videoDatabase;
        private IPodcastDatabase podcastDatabase;
        private IPlatformSettings platform;
        private IPodcastService podcastService;
        private LibVLC libVLC;
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaLibrary"/> class.
        /// </summary>
        /// <param name="libVLC">LibVLC Instance.</param>
        /// <param name="musicDatabase">Music Database.</param>
        /// <param name="videoDatabase">Video Database.</param>
        /// <param name="podcastDatabase">Podcast Database.</param>
        /// <param name="platform">Storage File APIs.</param>
        /// <param name="logger">Logger.</param>
        public MediaLibrary(LibVLC libVLC, IMusicDatabase musicDatabase, IVideoDatabase videoDatabase, IPodcastDatabase podcastDatabase, IPlatformSettings platform, ILogger logger)
        {
            this.platform = platform;
            this.logger = logger;
            this.libVLC = libVLC;
            this.podcastDatabase = podcastDatabase;
            this.musicDatabase = musicDatabase;
            this.videoDatabase = videoDatabase;
            this.podcastService = new PodcastService(this.logger);
            if (!this.musicDatabase.IsInitialized || !this.videoDatabase.IsInitialized || !this.podcastDatabase.IsInitialized)
            {
                throw new ArgumentException($"Databases must be initialized before using them in the media library.");
            }
        }

        /// <summary>
        /// Gets the new media item added event.
        /// </summary>
        public event EventHandler<NewMediaItemEventArgs>? NewMediaItemAdded;

        /// <summary>
        /// Gets the new media item added event.
        /// </summary>
        public event EventHandler<UpdateMediaItemEventArgs>? UpdateMediaItemAdded;

        /// <summary>
        /// Gets the remove media item event.
        /// </summary>
        public event EventHandler<RemoveMediaItemEventArgs>? RemoveMediaItem;

        /// <summary>
        /// Gets the new media item error event.
        /// </summary>
        public event EventHandler<NewMediaItemErrorEventArgs>? NewMediaItemError;

        /// <summary>
        /// Gets the music database.
        /// </summary>
        public IMusicDatabase MusicDatabase => this.musicDatabase;

        /// <summary>
        /// Gets the video database.
        /// </summary>
        public IVideoDatabase VideoDatabase => this.videoDatabase;

        /// <summary>
        /// Fetches all VideoItems.
        /// </summary>
        /// <returns>VideoItem.</returns>
        public Task<List<VideoItem>> FetchVideosAsync() => this.videoDatabase.FetchVideosAsync();

        /// <summary>
        /// Fetches all TV Shows.
        /// </summary>
        /// <returns>TVShows.</returns>
        public Task<List<TVShow>> FetchTVShowsAsync() => this.videoDatabase.FetchTVShowsAsync();

        /// <summary>
        /// Fetches a TV Show with episodes.
        /// </summary>
        /// <param name="id">TVShow id.</param>
        /// <returns>TVShow.</returns>
        public Task<TVShow?> FetchTVShowWithEpisodesAsync(int id) => this.videoDatabase.FetchTVShowWithEpisodesAsync(id);

        /// <summary>
        /// Fetches a TV Show via name.
        /// </summary>
        /// <param name="name">TVShow name.</param>
        /// <returns>TVShow.</returns>
        public Task<TVShow?> FetchTVShowViaNameAsync(string name) => this.videoDatabase.FetchTVShowViaNameAsync(name);

        /// <summary>
        /// Recursivly scan media directories.
        /// </summary>
        /// <param name="mediaDirectory">Starting directory.</param>
        /// <returns>Task.</returns>
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

        /// <summary>
        /// Scan media directory.
        /// </summary>
        /// <param name="mediaDirectory">Directory.</param>
        /// <returns>Task.</returns>
        public async Task ScanMediaDirectoryAsync(string mediaDirectory)
        {
            var files = Directory.EnumerateFiles(mediaDirectory);
            foreach (var file in files)
            {
                var result = await this.AddFileAsync(file);
                this.logger.Log(LogLevel.Debug, $"ScanMediaDirectory: {file} - {result}");
            }
        }

        /// <summary>
        /// Fetch an album with tracks by the album id.
        /// </summary>
        /// <param name="id">Album Id.</param>
        /// <returns>AlbumItem.</returns>
        public async Task<AlbumItem?> FetchAlbumWithTracksViaIdAsync(int id) => await this.musicDatabase.FetchAlbumWithTracksViaIdAsync(id);

        /// <summary>
        /// Gets an list of <see cref="ArtistItem"/>.
        /// </summary>
        /// <returns>List of ArtistItems.</returns>
        public async Task<List<ArtistItem>> FetchArtistsAsync() => await this.musicDatabase.FetchArtistsAsync();

        /// <summary>
        /// Fetch an artist by the artist id.
        /// </summary>
        /// <param name="id">Artist Id.</param>
        /// <returns>ArtistItem.</returns>
        public async Task<ArtistItem?> FetchArtistViaIdAsync(int id) => await this.musicDatabase.FetchArtistViaIdAsync(id);

        /// <summary>
        /// Fetch an artist with albums by the artist id.
        /// </summary>
        /// <param name="id">Artist Id.</param>
        /// <returns>ArtistItem.</returns>
        public async Task<ArtistItem?> FetchArtistWithAlbumsViaIdAsync(int id) => await this.musicDatabase.FetchArtistWithAlbumsViaIdAsync(id);

        /// <summary>
        /// Gets an <see cref="ArtistItem"/> via their Artist or ArtistGroup name.
        /// </summary>
        /// <param name="name">Name to search for.</param>
        /// <returns>ArtistItem.</returns>
        public async Task<ArtistItem?> FetchArtistViaNameAsync(string name) => await this.musicDatabase.FetchArtistViaNameAsync(name);

        /// <summary>
        /// Gets an list of <see cref="AlbumItem"/>.
        /// </summary>
        /// <returns>List of AlbumItems.</returns>
        public async Task<List<AlbumItem>> FetchAlbumsAsync() => await this.musicDatabase.FetchAlbumsAsync();

        /// <summary>
        /// Fetch an album by the album id.
        /// </summary>
        /// <param name="id">Album Id.</param>
        /// <returns>AlbumItem.</returns>
        public async Task<AlbumItem?> FetchAlbumViaIdAsync(int id) => await this.musicDatabase.FetchAlbumViaIdAsync(id);

        /// <summary>
        /// Fetch an track by the track id.
        /// </summary>
        /// <param name="id">Track Id.</param>
        /// <returns>TrackItem.</returns>
        public async Task<TrackItem?> FetchTrackViaIdAsync(int id) => await this.musicDatabase.FetchTrackViaIdAsync(id);

        /// <summary>
        /// Gets an list of <see cref="TrackItem"/>.
        /// </summary>
        /// <returns>List of TrackItems.</returns>
        public async Task<List<TrackItem>> FetchTracksAsync() => await this.musicDatabase.FetchTracksAsync();

        /// <summary>
        /// Remove TVShow from database.
        /// </summary>
        /// <param name="show">TVShow to remove.</param>
        /// <returns>Task.</returns>
        public async Task RemoveTVShowAsync(TVShow show)
        {
            show = await this.videoDatabase.RemoveTVShowAsync(show).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(show));
        }

        /// <summary>
        /// Remove VideoItem from database.
        /// </summary>
        /// <param name="videoItem">VideoItem to remove.</param>
        /// <returns>Task.</returns>
        public async Task RemoveVideoItem(VideoItem videoItem)
        {
            videoItem = await this.videoDatabase.RemoveVideoItemAsync(videoItem).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(videoItem));
            if (videoItem.TvShow is not null && videoItem.TvShow.Episodes.Count <= 0)
            {
                await this.RemoveTVShowAsync(videoItem.TvShow).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Remove Podcast from Database.
        /// </summary>
        /// <param name="podcast">PodcastShowItem</param>
        /// <returns>Task.</returns>
        public async Task RemovePodcast(PodcastShowItem podcast)
        {
            podcast = await this.podcastDatabase.RemovePodcastAsync(podcast).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(podcast));
        }

        /// <summary>
        /// Remove Podcast from Database.
        /// </summary>
        /// <param name="podcast">PodcastEpisodeItem</param>
        /// <returns>Task.</returns>
        public async Task RemovePodcastEpisode(PodcastEpisodeItem podcast)
        {
            podcast = await this.podcastDatabase.RemoveEpisodeAsync(podcast).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(podcast));
        }

        /// <summary>
        /// Remove Artist from database.
        /// </summary>
        /// <param name="artist">ArtistItem.</param>
        /// <returns>Task.</returns>
        public async Task RemoveArtistAsync(ArtistItem artist)
        {
            artist = await this.musicDatabase.RemoveArtistAsync(artist).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(artist));
        }

        /// <summary>
        /// Remove Album from database.
        /// </summary>
        /// <param name="album">AlbumItem.</param>
        /// <returns>Task.</returns>
        public async Task RemoveAlbumAsync(AlbumItem album)
        {
            album = await this.musicDatabase.RemoveAlbumAsync(album).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(album));
            if (album.ArtistItem?.Albums != null && album.ArtistItem.Albums.Count <= 0)
            {
                await this.RemoveArtistAsync(album.ArtistItem).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Remove Track from database.
        /// </summary>
        /// <param name="track">TrackItem.</param>
        /// <returns>Task.</returns>
        public async Task RemoveTrackAsync(TrackItem track)
        {
            track = await this.musicDatabase.RemoveTrackAsync(track).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(track));
            if (track.AlbumItem?.Tracks != null && track.AlbumItem.Tracks.Count <= 0)
            {
                await this.RemoveAlbumAsync(track.AlbumItem).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Fetch all podcasts.
        /// </summary>
        /// <returns>List of PodcastShowItem.</returns>
        public async Task<List<PodcastShowItem>> FetchPodcastsAsync() => await this.podcastDatabase.FetchShowsAsync().ConfigureAwait(false);

        /// <summary>
        /// Fetch podcast with episodes.
        /// </summary>
        /// <param name="showId">Podcast show id.</param>
        /// <returns>PodcastShowItem</returns>
        public async Task<PodcastShowItem?> FetchPodcastWithEpisodesAsync(int showId) => await this.podcastDatabase.FetchShowWithEpisodesAsync(showId).ConfigureAwait(false);

        /// <summary>
        /// Add file to database async.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <returns>Bool if item was added to the database.</returns>
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

                    var mP = await this.libVLC.GetMusicPropertiesAsync(path) as TrackItem;
                    if (mP is null || (string.IsNullOrEmpty(mP.Artist) && string.IsNullOrEmpty(mP.Album) && string.IsNullOrEmpty(mP.Title)))
                    {
                        // We couldn't parse the file or the metadata is empty. Skip it.
                        this.OnNewMediaItemError(new NewMediaItemErrorEventArgs() { MediaItemPath = path });
                        return false;
                    }

                    var artistName = mP.Artist?.Trim();
                    var albumArtistName = mP.AlbumArtist?.Trim();
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

                    var videoItem = await this.libVLC.GetVideoPropertiesAsync(path).ConfigureAwait(false) as VideoItem;
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

        /// <summary>
        /// Add or update a podcast via a uri.
        /// </summary>
        /// <param name="uri">Uri of podcast.</param>
        /// <returns>Podcast Show Item.</returns>
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

        /// <summary>
        /// On New Media Item Added.
        /// </summary>
        /// <param name="e">NewMediaItemEventArgs.</param>
        protected virtual void OnNewMediaItemAdded(NewMediaItemEventArgs e)
        {
            this.NewMediaItemAdded?.Invoke(this, e);
        }

        /// <summary>
        /// On Update Media Item Added.
        /// </summary>
        /// <param name="e">UpdateMediaItemEventArgs.</param>
        protected virtual void OnUpdateMediaItemAdded(UpdateMediaItemEventArgs e)
        {
            this.UpdateMediaItemAdded?.Invoke(this, e);
        }

        /// <summary>
        /// On New Media Item Error.
        /// </summary>
        /// <param name="e">NewMediaItemEventArgs.</param>
        protected virtual void OnNewMediaItemError(NewMediaItemErrorEventArgs e)
        {
            this.NewMediaItemError?.Invoke(this, e);
        }

        /// <summary>
        /// On Media Item Removed.
        /// </summary>
        /// <param name="e">RemoveMediaItemEventArgs.</param>
        protected virtual void OnRemoveMediaItem(RemoveMediaItemEventArgs e)
        {
            this.RemoveMediaItem?.Invoke(this, e);
        }

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.libVLC.Dispose();
                }

                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
