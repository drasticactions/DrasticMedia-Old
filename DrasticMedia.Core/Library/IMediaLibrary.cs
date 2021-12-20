// <copyright file="IMediaLibrary.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticMedia.Core.Library
{
    /// <summary>
    /// Media Library.
    /// </summary>
    public interface IMediaLibrary : IDisposable
    {
        /// <summary>
        /// Gets the new media item added event.
        /// </summary>
        event EventHandler<NewMediaItemEventArgs>? NewMediaItemAdded;

        /// <summary>
        /// Gets the new media item added event.
        /// </summary>
        event EventHandler<UpdateMediaItemEventArgs>? UpdateMediaItemAdded;

        /// <summary>
        /// Gets the remove media item event.
        /// </summary>
        event EventHandler<RemoveMediaItemEventArgs>? RemoveMediaItem;

        /// <summary>
        /// Gets the new media item error event.
        /// </summary>
        event EventHandler<NewMediaItemErrorEventArgs>? NewMediaItemError;

        /// <summary>
        /// Fetches all VideoItems.
        /// </summary>
        /// <returns>VideoItem.</returns>
        Task<List<VideoItem>> FetchVideosAsync();

        /// <summary>
        /// Fetches all TV Shows.
        /// </summary>
        /// <returns>TVShows.</returns>
        Task<List<TVShow>> FetchTVShowsAsync();

        /// <summary>
        /// Fetches a TV Show with episodes.
        /// </summary>
        /// <param name="id">TVShow id.</param>
        /// <returns>TVShow.</returns>
        Task<TVShow?> FetchTVShowWithEpisodesAsync(int id);

        /// <summary>
        /// Fetches a TV Show via name.
        /// </summary>
        /// <param name="name">TVShow name.</param>
        /// <returns>TVShow.</returns>
        Task<TVShow?> FetchTVShowViaNameAsync(string name);

        /// <summary>
        /// Recursivly scan media directories.
        /// </summary>
        /// <param name="mediaDirectory">Starting directory.</param>
        /// <returns>Task.</returns>
        Task ScanMediaDirectoriesAsync(string mediaDirectory);

        /// <summary>
        /// Scan media directory.
        /// </summary>
        /// <param name="mediaDirectory">Directory.</param>
        /// <returns>Task.</returns>
        Task ScanMediaDirectoryAsync(string mediaDirectory);

        /// <summary>
        /// Fetch an artist by the artist id.
        /// </summary>
        /// <param name="id">Artist Id.</param>
        /// <returns>ArtistItem.</returns>
        Task<AlbumItem?> FetchAlbumWithTracksViaIdAsync(int id);

        /// <summary>
        /// Gets an list of <see cref="ArtistItem"/>.
        /// </summary>
        /// <returns>List of ArtistItems.</returns>
        Task<List<ArtistItem>> FetchArtistsAsync();

        /// <summary>
        /// Fetch an artist by the artist id.
        /// </summary>
        /// <param name="id">Artist Id.</param>
        /// <returns>ArtistItem.</returns>
        Task<ArtistItem?> FetchArtistViaIdAsync(int id);

        /// <summary>
        /// Fetch an artist with albums by the artist id.
        /// </summary>
        /// <param name="id">Artist Id.</param>
        /// <returns>ArtistItem.</returns>
        Task<ArtistItem?> FetchArtistWithAlbumsViaIdAsync(int id);

        /// <summary>
        /// Gets an <see cref="ArtistItem"/> via their Artist or ArtistGroup name.
        /// </summary>
        /// <param name="name">Name to search for.</param>
        /// <returns>ArtistItem.</returns>
        Task<ArtistItem?> FetchArtistViaNameAsync(string name);

        /// <summary>
        /// Gets an list of <see cref="AlbumItem"/>.
        /// </summary>
        /// <returns>List of AlbumItems.</returns>
        Task<List<AlbumItem>> FetchAlbumsAsync();

        /// <summary>
        /// Fetch an album by the album id.
        /// </summary>
        /// <param name="id">Album Id.</param>
        /// <returns>AlbumItem.</returns>
        Task<AlbumItem?> FetchAlbumViaIdAsync(int id);

        /// <summary>
        /// Fetch an track by the track id.
        /// </summary>
        /// <param name="id">Track Id.</param>
        /// <returns>TrackItem.</returns>
        Task<TrackItem?> FetchTrackViaIdAsync(int id);

        /// <summary>
        /// Gets an list of <see cref="TrackItem"/>.
        /// </summary>
        /// <returns>List of TrackItems.</returns>
        Task<List<TrackItem>> FetchTracksAsync();

        /// <summary>
        /// Remove TVShow from database.
        /// </summary>
        /// <param name="show">TVShow to remove.</param>
        /// <returns>Task.</returns>
        Task RemoveTVShowAsync(TVShow show);

        /// <summary>
        /// Remove VideoItem from database.
        /// </summary>
        /// <param name="videoItem">VideoItem to remove.</param>
        /// <returns>Task.</returns>
        Task RemoveVideoItem(VideoItem videoItem);

        /// <summary>
        /// Remove Podcast from Database.
        /// </summary>
        /// <param name="podcast">PodcastShowItem.</param>
        /// <returns>Task.</returns>
        Task RemovePodcast(PodcastShowItem podcast);

        /// <summary>
        /// Remove Podcast from Database.
        /// </summary>
        /// <param name="podcast">PodcastShowItem.</param>
        /// <returns>Task.</returns>
        Task RemovePodcastEpisode(PodcastEpisodeItem podcast);

        /// <summary>
        /// Remove Artist from database.
        /// </summary>
        /// <param name="artist">ArtistItem.</param>
        /// <returns>Task.</returns>
        Task RemoveArtistAsync(ArtistItem artist);

        /// <summary>
        /// Remove Album from database.
        /// </summary>
        /// <param name="album">AlbumItem.</param>
        /// <returns>Task.</returns>
        Task RemoveAlbumAsync(AlbumItem album);

        /// <summary>
        /// Remove Track from database.
        /// </summary>
        /// <param name="track">TrackItem.</param>
        /// <returns>Task.</returns>
        Task RemoveTrackAsync(TrackItem track);

        /// <summary>
        /// Fetch all podcasts.
        /// </summary>
        /// <returns>List of PodcastShowItem.</returns>
        Task<List<PodcastShowItem>> FetchPodcastsAsync();

        /// <summary>
        /// Fetch podcast with episodes.
        /// </summary>
        /// <param name="showId">Podcast show id.</param>
        /// <returns>PodcastShowItem.</returns>
        Task<PodcastShowItem?> FetchPodcastWithEpisodesAsync(int showId);

        /// <summary>
        /// Add file to database async.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <returns>Bool if item was added to the database.</returns>
        Task<bool> AddFileAsync(string path);

        /// <summary>
        /// Add or update a podcast via a uri.
        /// </summary>
        /// <param name="uri">Uri of podcast.</param>
        /// <returns>Podcast Show Item.</returns>
        Task<PodcastShowItem?> AddOrUpdatePodcastFromUri(Uri uri);

        /// <summary>
        /// On New Media Item Added.
        /// </summary>
        /// <param name="e">NewMediaItemEventArgs.</param>
        void OnNewMediaItemAdded(NewMediaItemEventArgs e);

        /// <summary>
        /// On Update Media Item Added.
        /// </summary>
        /// <param name="e">UpdateMediaItemEventArgs.</param>
        void OnUpdateMediaItemAdded(UpdateMediaItemEventArgs e);

        /// <summary>
        /// On New Media Item Error.
        /// </summary>
        /// <param name="e">NewMediaItemEventArgs.</param>
        void OnNewMediaItemError(NewMediaItemErrorEventArgs e);

        /// <summary>
        /// On Media Item Removed.
        /// </summary>
        /// <param name="e">RemoveMediaItemEventArgs.</param>
        void OnRemoveMediaItem(RemoveMediaItemEventArgs e);
    }
}
