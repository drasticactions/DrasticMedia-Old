// <copyright file="PodcastDatabase.cs" company="Drastic Actions">
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
    /// Podcast Database.
    /// </summary>
    public class PodcastDatabase : DbContext, IPodcastDatabase
    {
        private string dbPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastDatabase"/> class.
        /// </summary>
        /// <param name="dbPath">Path to Database File.</param>
        public PodcastDatabase(string dbPath)
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                throw new ArgumentNullException(nameof(dbPath));
            }

            this.dbPath = dbPath;
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastDatabase"/> class.
        /// </summary>
        /// <param name="settings">Platform Settings.</param>
        public PodcastDatabase(IPlatformSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.dbPath = System.IO.Path.Combine(settings.DatabasePath, "drastic.podcast.db");
            this.Initialize();
        }

        /// <inheritdoc/>
        public bool IsInitialized { get; set; }

        /// <summary>
        /// Gets or sets the Shows table.
        /// </summary>
        public DbSet<PodcastShowItem> Shows { get; set; }

        /// <summary>
        /// Gets or sets the Episodes table.
        /// </summary>
        public DbSet<PodcastEpisodeItem> Episodes { get; set; }

        public async Task<PodcastEpisodeItem> AddEpisodeAsync(PodcastEpisodeItem episode)
        {
            await this.Episodes.AddAsync(episode);
            await this.SaveChangesAsync();
            return episode;
        }

        /// <inheritdoc/>
        public async Task<List<PodcastEpisodeItem>> AddEpisodesAsync(List<PodcastEpisodeItem> episodes)
        {
            await this.Episodes.AddRangeAsync(episodes);
            await this.SaveChangesAsync();
            return episodes;
        }

        /// <inheritdoc/>
        public async Task<PodcastShowItem> AddPodcastAsync(PodcastShowItem podcast)
        {
            await this.Shows.AddAsync(podcast);
            await this.SaveChangesAsync();
            return podcast;
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
        public Task<List<PodcastEpisodeItem>> FetchAllEpisodesAsync()
        {
            return this.Episodes.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<PodcastShowItem> FetchShowAsync(int showId)
        {
            return await this.Shows.FirstOrDefaultAsync(n => n.Id == showId).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<PodcastShowItem> FetchShowWithEpisodesAsync(int showId)
        {
            return await this.Shows.Include(n => n.Episodes).FirstOrDefaultAsync(n => n.Id == showId).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<PodcastEpisodeItem> FetchEpisodeAsync(int episodeId)
        {
            return await this.Episodes.FirstOrDefaultAsync(n => n.Id == episodeId).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<List<PodcastEpisodeItem>> FetchEpisodesAsync(int showId)
        {
            return await this.Episodes.Where(n => n.PodcastShowId == showId).ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public Task<List<PodcastShowItem>> FetchShowsAsync()
        {
            return this.Shows.ToListAsync();
        }

        /// <inheritdoc/>
        public void Initialize()
        {
            this.Database.EnsureCreated();
            this.IsInitialized = true;
        }

        /// <inheritdoc/>
        public async Task<PodcastEpisodeItem> RemoveEpisodeAsync(PodcastEpisodeItem episode)
        {
            episode = await this.Episodes.Include(n => n.PodcastShowItem).FirstAsync(n => n.Id == episode.Id);
            this.Episodes.Remove(episode);
            var rows = await this.SaveChangesAsync();
            return episode;
        }

        /// <inheritdoc/>
        public async Task<PodcastShowItem> RemovePodcastAsync(PodcastShowItem show)
        {
            show = await this.Shows.Include(n => n.Episodes).FirstAsync(n => n.Id == show.Id);
            this.Shows.Remove(show);
            var rows = await this.SaveChangesAsync();
            return show;
        }

        /// <inheritdoc/>
        public async Task<PodcastEpisodeItem> UpdateEpisodeAsync(PodcastEpisodeItem episode)
        {
            if (episode.Id > 0)
            {
                throw new ArgumentException($"{nameof(episode)} has id greater than 0");
            }

            this.Episodes.Update(episode);
            await this.SaveChangesAsync();
            return episode;
        }

        /// <inheritdoc/>
        public async Task<PodcastShowItem> UpdatePodcastAsync(PodcastShowItem podcast)
        {
            if (podcast.Id > 0)
            {
                throw new ArgumentException($"{nameof(podcast)} has id greater than 0");
            }

            this.Shows.Update(podcast);
            await this.SaveChangesAsync();
            return podcast;
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
        }
    }
}
