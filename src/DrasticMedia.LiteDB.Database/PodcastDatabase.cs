// <copyright file="PodcastDatabase.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Database;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Platform;
using LiteDB;

namespace DrasticMedia.LiteDB.Database
{
    /// <summary>
    /// LiteDB Podcast Database.
    /// </summary>
    public class PodcastDatabase : IPodcastDatabase
    {
        private string dbPath;
        private LiteDatabase? db;

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

            this.dbPath = System.IO.Path.Combine(settings.DatabasePath, "drastic.litedb.podcast.db");
            this.Initialize();
        }

        /// <summary>
        /// Gets the list of episodes.
        /// </summary>
        public ILiteCollection<PodcastEpisodeItem> Episodes => this.db?.GetCollection<PodcastEpisodeItem>(nameof(this.Episodes)) ?? throw new NullReferenceException(nameof(this.db));

        /// <summary>
        /// Gets the list of Shows.
        /// </summary>
        public ILiteCollection<PodcastShowItem> Shows => this.db?.GetCollection<PodcastShowItem>(nameof(this.Shows)) ?? throw new NullReferenceException(nameof(this.db));

        /// <inheritdoc/>
        public bool IsInitialized { get; set; }

        /// <inheritdoc/>
        public Task<PodcastEpisodeItem> AddEpisodeAsync(PodcastEpisodeItem episode)
        {
            this.Episodes.Insert(episode);
            return Task.FromResult(episode);
        }

        /// <inheritdoc/>
        public Task<List<PodcastEpisodeItem>> AddEpisodesAsync(List<PodcastEpisodeItem> episodes)
        {
            this.Episodes.InsertBulk(episodes);
            return Task.FromResult(episodes);
        }

        /// <inheritdoc/>
        public async Task<PodcastShowItem> AddPodcastAsync(PodcastShowItem podcast)
        {
            await this.AddEpisodesAsync(podcast.Episodes);

            this.Shows.Insert(podcast);
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
            this.db?.DropCollection(nameof(this.Episodes));
            this.db?.DropCollection(nameof(this.Shows));
        }

        /// <inheritdoc/>
        public Task<List<PodcastEpisodeItem>> FetchAllEpisodesAsync()
        {
            return Task.FromResult(this.Episodes.FindAll().ToList());
        }

        /// <inheritdoc/>
        public Task<PodcastEpisodeItem> FetchEpisodeAsync(int episodeId)
        {
            return Task.FromResult(this.Episodes.FindOne(n => n.Id == episodeId));
        }

        /// <inheritdoc/>
        public Task<List<PodcastEpisodeItem>> FetchEpisodesAsync(int showId)
        {
            return Task.FromResult(this.Episodes.Find(n => n.PodcastShowItemId == showId).ToList());
        }

        /// <inheritdoc/>
        public Task<PodcastShowItem> FetchShowAsync(int showId)
        {
            return Task.FromResult(this.Shows.FindOne(n => n.Id == showId));
        }

        /// <inheritdoc/>
        public Task<List<PodcastShowItem>> FetchShowsAsync()
        {
            return Task.FromResult(this.Shows.Include(n => n.Episodes).FindAll().ToList());
        }

        /// <inheritdoc/>
        public Task<PodcastShowItem> FetchShowViaUriAsync(Uri showUri)
        {
            return Task.FromResult(this.Shows.FindOne(n => n.SiteUri == showUri));
        }

        /// <inheritdoc/>
        public Task<PodcastShowItem> FetchShowWithEpisodesAsync(int showId)
        {
            return Task.FromResult(this.Shows.Include(n => n.Episodes).FindOne(n => n.Id == showId));
        }

        /// <inheritdoc/>
        public void Initialize()
        {
            if (!Directory.Exists(Path.GetDirectoryName(this.dbPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(this.dbPath));
            }

            this.db = new LiteDatabase(this.dbPath);
            global::LiteDB.BsonMapper.Global.Entity<PodcastEpisodeItem>().Id(n => n.Id).DbRef(x => x.PodcastShowItem, nameof(this.Shows));
            global::LiteDB.BsonMapper.Global.Entity<PodcastShowItem>().Id(n => n.Id).DbRef(x => x.Episodes, nameof(this.Episodes));
            this.IsInitialized = true;
        }

        /// <inheritdoc/>
        public Task<PodcastEpisodeItem> RemoveEpisodeAsync(PodcastEpisodeItem episode)
        {
            var result = this.Episodes.DeleteMany(n => n.Id == episode.Id);
            return Task.FromResult(episode);
        }

        /// <inheritdoc/>
        public Task<PodcastShowItem> RemovePodcastAsync(PodcastShowItem show)
        {
            var result = this.Shows.DeleteMany(n => n.Id == show.Id);
            return Task.FromResult(show);
        }

        /// <inheritdoc/>
        public Task<PodcastEpisodeItem> UpdateEpisodeAsync(PodcastEpisodeItem episode)
        {
            this.Episodes.Upsert(episode);
            return Task.FromResult(episode);
        }

        /// <inheritdoc/>
        public Task<PodcastShowItem> UpdatePodcastAsync(PodcastShowItem podcast)
        {
            this.Shows.Upsert(podcast);
            return Task.FromResult(podcast);
        }
    }
}
