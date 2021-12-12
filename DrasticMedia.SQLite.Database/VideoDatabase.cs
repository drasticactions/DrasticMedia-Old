// <copyright file="VideoDatabase.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Database;
using DrasticMedia.Core.Exceptions;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Platform;
using LibVLCSharp.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DrasticMedia.SQLite.Database
{
    /// <summary>
    /// Video Database.
    /// </summary>
    public class VideoDatabase : DbContext, IVideoDatabase
    {
        private string dbPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoDatabase"/> class.
        /// VLC Video Database.
        /// </summary>
        /// <param name="dbPath">Path to Database File.</param>
        public VideoDatabase(string dbPath)
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                throw new ArgumentNullException(nameof(dbPath));
            }

            this.dbPath = dbPath;
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoDatabase"/> class.
        /// VLC Video Database.
        /// </summary>
        /// <param name="settings">Platform Settings.</param>
        public VideoDatabase(IPlatformSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.dbPath = System.IO.Path.Combine(settings.DatabasePath, "vlc.video.db");
            this.Initialize();
        }

        /// <inheritdoc/>
        public bool IsInitialized { get; set; }

        /// <summary>
        /// Gets or sets the Videos table.
        /// </summary>
        public DbSet<VideoItem> Videos { get; set; }

        /// <summary>
        /// Gets or sets the TVShows table.
        /// </summary>
        public DbSet<TVShow> TVShows { get; set; }

        /// <inheritdoc/>
        public async Task<TVShow> AddTVShowAsync(TVShow show)
        {
            if (show.Id > 0)
            {
                throw new ArgumentException($"{nameof(show)} has id greater than 0");
            }

            await this.TVShows.AddAsync(show);
            await this.SaveChangesAsync();
            return show;
        }

        /// <inheritdoc/>
        public async Task<VideoItem> AddVideoItemAsync(VideoItem video)
        {
            if (video.Id > 0)
            {
                throw new ArgumentException($"{nameof(video)} has id greater than 0");
            }

            await this.Videos.AddAsync(video);
            await this.SaveChangesAsync();
            return video;
        }

        /// <inheritdoc/>
        public Task<bool> ContainsVideoAsync(string path)
        {
            return this.Videos.AnyAsync(n => n.Path == path);
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
        public Task<List<TVShow>> FetchTVShowsAsync()
        {
            return this.TVShows.Include(n => n.Episodes).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<TVShow> FetchTVShowViaNameAsync(string name)
        {
            return await this.TVShows.FirstOrDefaultAsync(n => n.ShowTitle.Equals(name)).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public Task<TVShow> FetchTVShowWithEpisodesAsync(int id)
        {
            return this.TVShows.Include(n => n.Episodes).FirstOrDefaultAsync(n => n.Id == id);
        }

        /// <inheritdoc/>
        public void Initialize()
        {
            this.Database.EnsureCreated();
            this.IsInitialized = true;
        }

        /// <inheritdoc/>
        public async Task<TVShow> RemoveTVShowAsync(TVShow show)
        {
            show = await this.TVShows.Include(n => n.Episodes).FirstAsync(n => n.Id == show.Id).ConfigureAwait(false);
            await this.SaveChangesAsync().ConfigureAwait(false);
            return show;
        }

        /// <inheritdoc/>
        public async Task<VideoItem> RemoveVideoItemAsync(VideoItem video)
        {
            video = await this.Videos.Include(n => n.TvShow).FirstAsync(n => n.Id == video.Id).ConfigureAwait(false);
            await this.SaveChangesAsync().ConfigureAwait(false);
            return video;
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

            modelBuilder.Entity<VideoItem>().HasKey(n => n.Id);
            modelBuilder.Entity<TVShow>().HasKey(n => n.Id);
            modelBuilder.Entity<VideoItem>().HasOne(n => n.TvShow).WithMany(y => y.Episodes).HasForeignKey(n => n.TvShowId);
            modelBuilder.Entity<TVShow>().HasMany(n => n.Episodes).WithOne().HasForeignKey(y => y.TvShowId);
        }
    }
}
