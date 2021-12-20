﻿// <copyright file="MusicDatabase.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Database;
using DrasticMedia.Core.Exceptions;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Platform;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DrasticMedia.SQLite.Database
{
    /// <summary>
    /// Music Database.
    /// </summary>
    public class MusicDatabase : DbContext, IMusicDatabase
    {
        private string dbPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicDatabase"/> class.
        /// VLC Music Database.
        /// </summary>
        /// <param name="dbPath">Path to Database File.</param>
        public MusicDatabase(string dbPath)
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                throw new ArgumentNullException(nameof(dbPath));
            }

            this.dbPath = dbPath;
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicDatabase"/> class.
        /// VLC Music Database.
        /// </summary>
        /// <param name="settings">Platform Settings.</param>
        public MusicDatabase(IPlatformSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.dbPath = System.IO.Path.Combine(settings.DatabasePath, "drastic.music.db");
            this.Initialize();
        }

        /// <inheritdoc/>
        public bool IsInitialized { get; set; }

        /// <summary>
        /// Gets or sets the Artists Table.
        /// </summary>
        public DbSet<ArtistItem> Artists { get; set; }

        /// <summary>
        /// Gets or sets the Albums table.
        /// </summary>
        public DbSet<AlbumItem> Albums { get; set; }

        /// <summary>
        /// Gets or sets the Tracks table.
        /// </summary>
        public DbSet<TrackItem> Tracks { get; set; }

        /// <inheritdoc/>
        public async Task<AlbumItem> AddAlbumAsync(AlbumItem album)
        {
            await this.Albums.AddAsync(album);
            await this.SaveChangesAsync();
            return album;
        }

        /// <inheritdoc/>
        public async Task<ArtistItem> AddArtistAsync(ArtistItem artist)
        {
            await this.Artists.AddAsync(artist);
            await this.SaveChangesAsync();
            return artist;
        }

        /// <inheritdoc/>
        public async Task<TrackItem> AddTrackAsync(TrackItem track)
        {
            await this.Tracks.AddAsync(track);
            await this.SaveChangesAsync();
            return track;
        }

        /// <inheritdoc/>
        public async Task<AlbumItem> UpdateAlbumAsync(AlbumItem album)
        {
            if (album.Id > 0)
            {
                throw new ArgumentException($"{nameof(album)} has id greater than 0");
            }

            this.Albums.Update(album);
            await this.SaveChangesAsync();
            return album;
        }

        /// <inheritdoc/>
        public async Task<ArtistItem> UpdateArtistAsync(ArtistItem artist)
        {
            if (artist.Id > 0)
            {
                throw new ArgumentException($"{nameof(artist)} has id greater than 0");
            }

            this.Artists.Update(artist);
            await this.SaveChangesAsync();
            return artist;
        }

        /// <inheritdoc/>
        public async Task<TrackItem> UpdateTrackAsync(TrackItem track)
        {
            if (track.Id > 0)
            {
                throw new ArgumentException($"{nameof(track)} has id greater than 0");
            }

            this.Tracks.Update(track);
            await this.SaveChangesAsync();
            return track;
        }

        /// <inheritdoc/>
        public Task<bool> ContainsTrackAsync(string path)
        {
            return this.Tracks.AnyAsync(n => n.Path.Equals(path));
        }

        /// <inheritdoc/>
        public void DeleteAll()
        {
            this.Drop();
        }

        /// <inheritdoc/>
        public void Drop()
        {
#pragma warning disable EF1001 // Internal EF Core API usage.
            this.GetDependencies().StateManager.ResetState();
#pragma warning restore EF1001 // Internal EF Core API usage.
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
        }

        /// <inheritdoc/>
        public Task<List<AlbumItem>> FetchAlbumsAsync()
        {
            return this.Albums.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<AlbumItem?> FetchAlbumViaIdAsync(int id)
        {
            return await this.Albums.FirstOrDefaultAsync(n => n.Id == id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public Task<AlbumItem?> FetchAlbumViaNameAsync(int artistId, string name)
        {
            return this.Albums.FirstOrDefaultAsync(n => n.Name.Equals(name) && n.ArtistItemId == artistId);
        }

        /// <inheritdoc/>
        public async Task<AlbumItem?> FetchAlbumWithTracksViaIdAsync(int id)
        {
            return await this.Albums.Include(n => n.Tracks).FirstOrDefaultAsync(n => n.Id == id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public Task<List<ArtistItem>> FetchArtistsAsync()
        {
            return this.Artists.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ArtistItem?> FetchArtistViaIdAsync(int id)
        {
            return await this.Artists.FirstOrDefaultAsync(n => n.Id == id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ArtistItem?> FetchArtistViaNameAsync(string name)
        {
            return await this.Artists.FirstOrDefaultAsync(n => n.Name.Equals(name)).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ArtistItem?> FetchArtistWithAlbumsViaIdAsync(int id)
        {
            return await this.Artists.Include(n => n.Albums).FirstOrDefaultAsync(n => n.Id == id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public Task<List<TrackItem>> FetchTracksAsync()
        {
            return this.Tracks.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<TrackItem?> FetchTrackViaIdAsync(int id)
        {
            return await this.Tracks.FirstOrDefaultAsync(n => n.Id == id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public void Initialize()
        {
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(this.dbPath));
            this.Database.EnsureCreated();
            this.IsInitialized = true;
        }

        /// <inheritdoc/>
        public async Task<AlbumItem> RemoveAlbumAsync(AlbumItem album)
        {
            album = await this.Albums.Include(n => n.ArtistItem).Include(n => n.Tracks).FirstAsync(n => n.Id == album.Id);
            this.Albums.Remove(album);
            var rows = await this.SaveChangesAsync();
            return album;
        }

        /// <inheritdoc/>
        public async Task<ArtistItem> RemoveArtistAsync(ArtistItem artist)
        {
            artist = await this.Artists.Include(n => n.Albums).Include(n => n.Tracks).FirstAsync(n => n.Id == artist.Id);
            this.Artists.Remove(artist);
            var rows = await this.SaveChangesAsync();
            return artist;
        }

        /// <inheritdoc/>
        public async Task<TrackItem> RemoveTrackAsync(TrackItem track)
        {
            track = await this.Tracks.Include(n => n.AlbumItem).Include(n => n.ArtistItem).FirstAsync(n => n.Id == track.Id);
            this.Tracks.Remove(track);
            var rows = await this.SaveChangesAsync();
            return track;
        }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={this.dbPath};");
            optionsBuilder.EnableSensitiveDataLogging();
        }

        /// <summary>
        /// Run when building the model.
        /// </summary>
        /// <param name="modelBuilder"><see cref="ModelBuilder"/>.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<ArtistItem>().HasKey(n => n.Id);
            modelBuilder.Entity<AlbumItem>().HasKey(n => n.Id);
            modelBuilder.Entity<TrackItem>().HasKey(n => n.Id);
            modelBuilder.Entity<ArtistItem>().HasMany(n => n.Albums).WithOne().HasForeignKey(y => y.ArtistItemId);
            modelBuilder.Entity<AlbumItem>().HasMany(n => n.Tracks).WithOne().HasForeignKey(y => y.AlbumItemId);
            modelBuilder.Entity<AlbumItem>().HasOne(n => n.ArtistItem).WithMany(n => n.Albums).HasForeignKey(n => n.ArtistItemId);
            modelBuilder.Entity<TrackItem>().HasOne(n => n.AlbumItem).WithMany(n => n.Tracks).HasForeignKey(n => n.AlbumItemId);
        }
    }
}
