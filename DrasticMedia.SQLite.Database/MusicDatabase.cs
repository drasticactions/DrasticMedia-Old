// <copyright file="MusicDatabase.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Database;
using DrasticMedia.Core.Exceptions;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Model.Metadata;
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
        /// Gets or sets the ArtistsSpotifyMetadata Table.
        /// </summary>
        public DbSet<ArtistSpotifyMetadata> ArtistsSpotifyMetadata { get; set; }

        /// <summary>
        /// Gets or sets the AlbumsSpotifyMetadata Table.
        /// </summary>
        public DbSet<AlbumSpotifyMetadata> AlbumsSpotifyMetadata { get; set; }

        /// <summary>
        /// Gets or sets the ArtistsLastFmMetadata Table.
        /// </summary>
        public DbSet<ArtistLastFmMetadata> ArtistsLastFmMetadata { get; set; }

        /// <summary>
        /// Gets or sets the AlbumsLastFmMetadata Table.
        /// </summary>
        public DbSet<AlbumLastFmMetadata> AlbumsLastFmMetadata { get; set; }

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
            if (album.Id > 0)
            {
                throw new ArgumentException($"{nameof(album)} has id greater than 0");
            }

            await this.Albums.AddAsync(album);
            await this.SaveChangesAsync();
            return album;
        }

        /// <inheritdoc/>
        public async Task<ArtistItem> AddArtistAsync(ArtistItem artist)
        {
            if (artist.Id > 0)
            {
                throw new ArgumentException($"{nameof(artist)} has id greater than 0");
            }

            await this.Artists.AddAsync(artist);
            await this.SaveChangesAsync();
            return artist;
        }

        /// <inheritdoc/>
        public async Task<ArtistSpotifyMetadata> AddArtistSpotifyMetadataAsync(ArtistSpotifyMetadata metadata)
        {
            if (metadata.Id > 0)
            {
                throw new ArgumentException($"{nameof(metadata)} has id greater than 0");
            }

            await this.ArtistsSpotifyMetadata.AddAsync(metadata);
            await this.SaveChangesAsync();
            return metadata;
        }

        /// <inheritdoc/>
        public async Task<AlbumSpotifyMetadata> AddAlbumSpotifyMetadataAsync(AlbumSpotifyMetadata metadata)
        {
            if (metadata.Id > 0)
            {
                throw new ArgumentException($"{nameof(metadata)} has id greater than 0");
            }

            await this.AlbumsSpotifyMetadata.AddAsync(metadata);
            await this.SaveChangesAsync();
            return metadata;
        }

        /// <inheritdoc/>
        public async Task<AlbumLastFmMetadata> AddAlbumLastFmMetadataAsync(AlbumLastFmMetadata metadata)
        {
            if (metadata.Id > 0)
            {
                throw new ArgumentException($"{nameof(metadata)} has id greater than 0");
            }

            await this.AlbumsLastFmMetadata.AddAsync(metadata);
            await this.SaveChangesAsync();
            return metadata;
        }

        /// <inheritdoc/>
        public async Task<ArtistLastFmMetadata> AddArtistLastFmMetadataAsync(ArtistLastFmMetadata metadata)
        {
            if (metadata.Id > 0)
            {
                throw new ArgumentException($"{nameof(metadata)} has id greater than 0");
            }

            await this.ArtistsLastFmMetadata.AddAsync(metadata);
            await this.SaveChangesAsync();
            return metadata;
        }

        /// <inheritdoc/>
        public async Task<ArtistSpotifyMetadata> UpdateArtistSpotifyMetadataAsync(ArtistSpotifyMetadata metadata)
        {
            this.ArtistsSpotifyMetadata.Update(metadata);
            await this.SaveChangesAsync();
            return metadata;
        }

        /// <inheritdoc/>
        public async Task<AlbumSpotifyMetadata> UpdateAlbumSpotifyMetadataAsync(AlbumSpotifyMetadata metadata)
        {
            this.AlbumsSpotifyMetadata.Update(metadata);
            await this.SaveChangesAsync();
            return metadata;
        }

        /// <inheritdoc/>
        public async Task<AlbumLastFmMetadata> UpdateAlbumLastFmMetadataAsync(AlbumLastFmMetadata metadata)
        {
            this.AlbumsLastFmMetadata.Update(metadata);
            await this.SaveChangesAsync();
            return metadata;
        }

        /// <inheritdoc/>
        public async Task<ArtistLastFmMetadata> UpdateArtistLastFmMetadataAsync(ArtistLastFmMetadata metadata)
        {
            this.ArtistsLastFmMetadata.Update(metadata);
            await this.SaveChangesAsync();
            return metadata;
        }

        /// <inheritdoc/>
        public async Task<TrackItem> AddTrackAsync(TrackItem track)
        {
            if (track.Id > 0)
            {
                throw new ArgumentException($"{nameof(track)} has id greater than 0");
            }

            await this.Tracks.AddAsync(track);
            await this.SaveChangesAsync();
            return track;
        }

        /// <inheritdoc/>
        public async Task<AlbumItem> UpdateAlbumAsync(AlbumItem album)
        {
            this.Albums.Update(album);
            await this.SaveChangesAsync();
            return album;
        }

        /// <inheritdoc/>
        public async Task<ArtistItem> UpdateArtistAsync(ArtistItem artist)
        {
            this.Artists.Update(artist);
            await this.SaveChangesAsync();
            return artist;
        }

        /// <inheritdoc/>
        public async Task<TrackItem> UpdateTrackAsync(TrackItem track)
        {
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
            return this.Albums.Include(n => n.LastFmMetadata).Include(n => n.SpotifyMetadata).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<AlbumItem?> FetchAlbumViaIdAsync(int id)
        {
            return await this.Albums.Include(n => n.LastFmMetadata).Include(n => n.SpotifyMetadata).FirstOrDefaultAsync(n => n.Id == id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public Task<AlbumItem?> FetchAlbumViaNameAsync(int artistId, string name)
        {
            return this.Albums.Include(n => n.LastFmMetadata).Include(n => n.SpotifyMetadata).FirstOrDefaultAsync(n => n.Name.Equals(name) && n.ArtistItemId == artistId);
        }

        /// <inheritdoc/>
        public async Task<AlbumItem?> FetchAlbumWithTracksViaIdAsync(int id)
        {
            return await this.Albums.Include(n => n.LastFmMetadata).Include(n => n.SpotifyMetadata).Include(n => n.Tracks).FirstOrDefaultAsync(n => n.Id == id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public Task<List<ArtistItem>> FetchArtistsAsync()
        {
            return this.Artists.Include(n => n.LastFmMetadata).Include(n => n.SpotifyMetadata).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ArtistItem?> FetchArtistViaIdAsync(int id)
        {
            return await this.Artists.Include(n => n.LastFmMetadata).Include(n => n.SpotifyMetadata).FirstOrDefaultAsync(n => n.Id == id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ArtistItem?> FetchArtistViaNameAsync(string name)
        {
            return await this.Artists.Include(n => n.LastFmMetadata).Include(n => n.SpotifyMetadata).FirstOrDefaultAsync(n => n.Name.Equals(name)).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ArtistItem?> FetchArtistWithAlbumsViaIdAsync(int id)
        {
            return await this.Artists.Include(n => n.LastFmMetadata).Include(n => n.SpotifyMetadata).Include(n => n.Albums).FirstOrDefaultAsync(n => n.Id == id).ConfigureAwait(false);
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
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
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
            modelBuilder.Entity<AlbumSpotifyMetadata>().HasKey(n => n.Id);
            modelBuilder.Entity<ArtistSpotifyMetadata>().HasKey(n => n.Id);
            modelBuilder.Entity<ArtistLastFmMetadata>().HasKey(n => n.Id);
            modelBuilder.Entity<AlbumLastFmMetadata>().HasKey(n => n.Id);
            modelBuilder.Entity<ArtistItem>().HasMany(n => n.Albums).WithOne().HasForeignKey(y => y.ArtistItemId);
            modelBuilder.Entity<ArtistItem>().HasOne(n => n.SpotifyMetadata).WithOne().HasForeignKey<ArtistSpotifyMetadata>(y => y.ArtistItemId);
            modelBuilder.Entity<ArtistItem>().HasOne(n => n.LastFmMetadata).WithOne().HasForeignKey<ArtistLastFmMetadata>(y => y.ArtistItemId);
            modelBuilder.Entity<AlbumItem>().HasMany(n => n.Tracks).WithOne().HasForeignKey(y => y.AlbumItemId);
            modelBuilder.Entity<AlbumItem>().HasOne(n => n.ArtistItem).WithMany(n => n.Albums).HasForeignKey(n => n.ArtistItemId);
            modelBuilder.Entity<AlbumItem>().HasOne(n => n.SpotifyMetadata).WithOne().HasForeignKey<AlbumSpotifyMetadata>(y => y.AlbumItemId);
            modelBuilder.Entity<AlbumItem>().HasOne(n => n.LastFmMetadata).WithOne().HasForeignKey<AlbumLastFmMetadata>(y => y.AlbumItemId);
            modelBuilder.Entity<TrackItem>().HasOne(n => n.AlbumItem).WithMany(n => n.Tracks).HasForeignKey(n => n.AlbumItemId);
        }
    }
}
