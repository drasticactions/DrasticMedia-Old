// <copyright file="IAudioLibrary.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Model;

namespace DrasticMedia.Core.Library
{
    /// <summary>
    /// Audio Library.
    /// </summary>
    public interface IAudioLibrary : IMediaLibrary
    {
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
        /// Add file to database async.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <returns>Bool if item was added to the database.</returns>
        Task<bool> AddFileAsync(string path);
    }
}
