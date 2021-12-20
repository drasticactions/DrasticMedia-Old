// <copyright file="SettingsDatabase.cs" company="Drastic Actions">
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
    /// Settings Database.
    /// </summary>
    public class SettingsDatabase : DbContext, ISettingsDatabase
    {
        private string dbPath;
        private IPlatformSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsDatabase"/> class.
        /// VLC Settings Database.
        /// </summary>
        /// <param name="dbPath">Path to Database File.</param>
        public SettingsDatabase(string dbPath)
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                throw new ArgumentNullException(nameof(dbPath));
            }

            this.dbPath = dbPath;
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsDatabase"/> class.
        /// VLC Settings Database.
        /// </summary>
        /// <param name="settings">Platform Settings.</param>
        public SettingsDatabase(IPlatformSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.dbPath = System.IO.Path.Combine(settings.DatabasePath, "vlc.settings.db");
            this.settings = settings;
            this.Initialize();
        }

        /// <inheritdoc/>
        public bool IsInitialized { get; set; }

        /// <summary>
        /// Gets or sets the AppSettings Table.
        /// </summary>
        public DbSet<AppSettings> AppSettings { get; set; }

        /// <summary>
        /// Gets or sets the MediaFolders Table.
        /// </summary>
        public DbSet<MediaFolder> MediaFolders { get; set; }

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
        public async Task<AppSettings> FetchAppSettingsAsync()
        {
            var settings = await this.AppSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new AppSettings()
                {
                    IsDarkMode = this.settings is not null && this.settings.IsDarkTheme,
                };
            }

            return settings;
        }

        /// <inheritdoc/>
        public Task<List<MediaFolder>> FetchMediaFoldersAsync()
        {
            return this.MediaFolders.ToListAsync();
        }

        /// <inheritdoc/>
        public void Initialize()
        {
            this.Database.EnsureCreated();
            this.IsInitialized = true;
        }

        /// <inheritdoc/>
        public async Task<AppSettings> SaveAppSettingsAsync(AppSettings settings)
        {
            if (settings.Id <= 0)
            {
                await this.AppSettings.AddAsync(settings);
            }
            else
            {
                this.AppSettings.Update(settings);
            }

            await this.SaveChangesAsync();
            return settings;
        }

        /// <inheritdoc/>
        public async Task<MediaFolder> SaveMediaFolderAsync(MediaFolder folder)
        {
            if (folder.Id > 0)
            {
                throw new ArgumentException($"{nameof(folder)} has id greater than 0");
            }

            await this.MediaFolders.AddAsync(folder);
            await this.SaveChangesAsync();
            return folder;
        }

        /// <inheritdoc/>
        public async Task<List<MediaFolder>> SaveMediaFoldersAsync(List<MediaFolder> folders)
        {
            if (folders.Any(n => n.Id > 0))
            {
                throw new ArgumentException($"{nameof(folders)} has id greater than 0");
            }

            await this.MediaFolders.AddRangeAsync(folders);
            await this.SaveChangesAsync();
            return folders;
        }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={this.dbPath};");
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
