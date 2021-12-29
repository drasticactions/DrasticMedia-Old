// <copyright file="IMusicDatabase.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Model.Metadata;

namespace DrasticMedia.Core.Database
{
    /// <summary>
    /// Music Database.
    /// </summary>
    public interface IMusicDatabase : IDatabase
    {
        /// <summary>
        /// Checks if the track exists in the database via the path.
        /// </summary>
        /// <param name="path">File path of the track.</param>
        /// <returns>Bool.</returns>
        Task<bool> ContainsTrackAsync(string path);

        /// <summary>
        /// Gets an list of <see cref="ArtistItem"/>.
        /// </summary>
        /// <returns>List of ArtistItems.</returns>
        Task<List<ArtistItem>> FetchArtistsAsync();

        /// <summary>
        /// Gets an list of <see cref="AlbumItem"/>.
        /// </summary>
        /// <returns>List of AlbumItems.</returns>
        Task<List<AlbumItem>> FetchAlbumsAsync();

        /// <summary>
        /// Gets an list of <see cref="TrackItem"/>.
        /// </summary>
        /// <returns>List of TrackItems.</returns>
        Task<List<TrackItem>> FetchTracksAsync();

        /// <summary>
        /// Gets an <see cref="ArtistItem"/> via their Artist or ArtistGroup name.
        /// </summary>
        /// <param name="name">Name to search for.</param>
        /// <returns>ArtistItem.</returns>
        Task<ArtistItem?> FetchArtistViaNameAsync(string name);

        /// <summary>
        /// Gets an <see cref="AlbumItem"/> via their AlbumItem and Artist Id.
        /// </summary>
        /// <param name="artistId">Artist Id.</param>
        /// <param name="name">Name to search for.</param>
        /// <returns>AlbumItem.</returns>
        Task<AlbumItem?> FetchAlbumViaNameAsync(int artistId, string name);

        /// <summary>
        /// Adds an artist to the database.
        /// </summary>
        /// <param name="artist">ArtistItem.</param>
        /// <returns>Updated ArtistItem with Id set.</returns>
        Task<ArtistItem> AddArtistAsync(ArtistItem artist);

        /// <summary>
        /// Adds an album to the database.
        /// </summary>
        /// <param name="album">AlbumItem.</param>
        /// <returns>Updated AlbumItem with Id set.</returns>
        Task<AlbumItem> AddAlbumAsync(AlbumItem album);

        /// <summary>
        /// Adds an track to the database.
        /// </summary>
        /// <param name="track">TrackItem.</param>
        /// <returns>Updated TrackItem with Id set.</returns>
        Task<TrackItem> AddTrackAsync(TrackItem track);

        /// <summary>
        /// Updates an artist to the database.
        /// </summary>
        /// <param name="artist">ArtistItem.</param>
        /// <returns>Updated ArtistItem with Id set.</returns>
        Task<ArtistItem> UpdateArtistAsync(ArtistItem artist);

        /// <summary>
        /// Updates an album to the database.
        /// </summary>
        /// <param name="album">AlbumItem.</param>
        /// <returns>Updated AlbumItem with Id set.</returns>
        Task<AlbumItem> UpdateAlbumAsync(AlbumItem album);

        /// <summary>
        /// Updates an track to the database.
        /// </summary>
        /// <param name="track">TrackItem.</param>
        /// <returns>Updated TrackItem with Id set.</returns>
        Task<TrackItem> UpdateTrackAsync(TrackItem track);

        /// <summary>
        /// Fetch an album by the album id.
        /// </summary>
        /// <param name="id">Album Id.</param>
        /// <returns>AlbumItem.</returns>
        Task<AlbumItem?> FetchAlbumViaIdAsync(int id);

        /// <summary>
        /// Fetch an album with tracks by the album id.
        /// </summary>
        /// <param name="id">Album Id.</param>
        /// <returns>AlbumItem.</returns>
        Task<AlbumItem?> FetchAlbumWithTracksViaIdAsync(int id);

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
        /// Fetch an track by the track id.
        /// </summary>
        /// <param name="id">Track Id.</param>
        /// <returns>TrackItem.</returns>
        Task<TrackItem?> FetchTrackViaIdAsync(int id);

        /// <summary>
        /// Remove artist from database.
        /// </summary>
        /// <param name="artist">ArtistItem.</param>
        /// <returns>Removed Item.</returns>
        Task<ArtistItem> RemoveArtistAsync(ArtistItem artist);

        /// <summary>
        /// Remove album from database.
        /// </summary>
        /// <param name="album">AlbumItem.</param>
        /// <returns>Removed Item.</returns>
        Task<AlbumItem> RemoveAlbumAsync(AlbumItem album);

        /// <summary>
        /// Remove track from database.
        /// </summary>
        /// <param name="track">TrackItem.</param>
        /// <returns>Removed Item.</returns>
        Task<TrackItem> RemoveTrackAsync(TrackItem track);

        /// <summary>
        /// Add Artist Spotify Metadata.
        /// </summary>
        /// <param name="metadata"><see cref="ArtistSpotifyMetadata"/>.</param>
        /// <returns>New <see cref="ArtistSpotifyMetadata"/>.</returns>
        Task<ArtistSpotifyMetadata> AddArtistSpotifyMetadataAsync(ArtistSpotifyMetadata metadata);

        /// <summary>
        /// Add Album Spotify Metadata.
        /// </summary>
        /// <param name="metadata"><see cref="AlbumSpotifyMetadata"/>.</param>
        /// <returns>New <see cref="AlbumSpotifyMetadata"/>.</returns>
        Task<AlbumSpotifyMetadata> AddAlbumSpotifyMetadataAsync(AlbumSpotifyMetadata metadata);

        /// <summary>
        /// Add Album Last FM Metadata.
        /// </summary>
        /// <param name="metadata"><see cref="AlbumLastFmMetadata"/>.</param>
        /// <returns>New <see cref="AlbumLastFmMetadata"/>.</returns>
        Task<AlbumLastFmMetadata> AddAlbumLastFmMetadataAsync(AlbumLastFmMetadata metadata);

        /// <summary>
        /// Add Artist LastFM Metadata.
        /// </summary>
        /// <param name="metadata"><see cref="ArtistLastFmMetadata"/>.</param>
        /// <returns>New <see cref="ArtistLastFmMetadata"/>.</returns>
        Task<ArtistLastFmMetadata> AddArtistLastFmMetadataAsync(ArtistLastFmMetadata metadata);

        /// <summary>
        /// Update Artist Spotify Metadata.
        /// </summary>
        /// <param name="metadata"><see cref="ArtistSpotifyMetadata"/>.</param>
        /// <returns>New <see cref="ArtistSpotifyMetadata"/>.</returns>
        Task<ArtistSpotifyMetadata> UpdateArtistSpotifyMetadataAsync(ArtistSpotifyMetadata metadata);

        /// <summary>
        /// Update Album Spotify Metadata.
        /// </summary>
        /// <param name="metadata"><see cref="AlbumSpotifyMetadata"/>.</param>
        /// <returns>New <see cref="AlbumSpotifyMetadata"/>.</returns>
        Task<AlbumSpotifyMetadata> UpdateAlbumSpotifyMetadataAsync(AlbumSpotifyMetadata metadata);

        /// <summary>
        /// Album LastFM Metadata.
        /// </summary>
        /// <param name="metadata"><see cref="AlbumLastFmMetadata"/>.</param>
        /// <returns>New <see cref="AlbumLastFmMetadata"/>.</returns>
        Task<AlbumLastFmMetadata> UpdateAlbumLastFmMetadataAsync(AlbumLastFmMetadata metadata);

        /// <summary>
        /// Update Artist LastFM Metadata.
        /// </summary>
        /// <param name="metadata"><see cref="ArtistLastFmMetadata"/>.</param>
        /// <returns>New <see cref="ArtistLastFmMetadata"/>.</returns>
        Task<ArtistLastFmMetadata> UpdateArtistLastFmMetadataAsync(ArtistLastFmMetadata metadata);
    }
}
