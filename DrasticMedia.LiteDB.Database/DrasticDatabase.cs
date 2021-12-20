// <copyright file="DrasticDatabase.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Database;
using DrasticMedia.Core.Exceptions;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Platform;
using LiteDB;

namespace DrasticMedia.LiteDB.Database
{
    /// <summary>
    /// Drastic Database.
    /// </summary>
    public class DrasticDatabase : ISettingsDatabase, IMusicDatabase, IVideoDatabase
    {
        private const string SettingsDB = "settings";
        private const string MusicDB = "music";
        private const string VideoDB = "video";

        private string? dbPath;
        private IPlatformSettings? settings;
        private readonly LiteDatabase db;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrasticDatabase"/> class.
        /// </summary>
        /// <param name="dbPath">Path to Database File.</param>
        public DrasticDatabase(string dbPath)
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                throw new ArgumentNullException(nameof(dbPath));
            }

            this.dbPath = dbPath;
            this.db = new LiteDatabase(this.dbPath);
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrasticDatabase"/> class.
        /// </summary>
        /// <param name="settings">Platform Settings.</param>
        public DrasticDatabase(IPlatformSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.dbPath = System.IO.Path.Combine(settings.DatabasePath, "drastic.litedb.db");
            this.db = new LiteDatabase(this.dbPath);
            this.settings = settings;
            this.Initialize();
        }

        /// <inheritdoc/>
        public bool IsInitialized { get; set; }

        /// <inheritdoc/>
        public void Initialize()
        {
            this.IsInitialized = true;
        }

        /// <inheritdoc/>
        public Task<AppSettings> FetchAppSettingsAsync()
        {
            var tcs = new TaskCompletionSource<AppSettings>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<AppSettings>(SettingsDB);
                var settings = collection.FindAll().FirstOrDefault();
                if (settings is null)
                {
                    settings = new AppSettings();
                }

                tcs.SetResult(settings);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<AppSettings> SaveAppSettingsAsync(AppSettings settings)
        {
            var tcs = new TaskCompletionSource<AppSettings>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<AppSettings>(SettingsDB);
                var result = collection.Upsert(settings);
                tcs.SetResult(settings);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<List<MediaFolder>> FetchMediaFoldersAsync()
        {
            var tcs = new TaskCompletionSource<List<MediaFolder>>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<MediaFolder>(SettingsDB);
                tcs.SetResult(collection.FindAll().ToList());
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<MediaFolder> SaveMediaFolderAsync(MediaFolder folder)
        {
            var tcs = new TaskCompletionSource<MediaFolder>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<MediaFolder>(SettingsDB);
                collection.Upsert(folder);
                tcs.SetResult(folder);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<List<MediaFolder>> SaveMediaFoldersAsync(List<MediaFolder> folders)
        {
            var tcs = new TaskCompletionSource<List<MediaFolder>>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<MediaFolder>(SettingsDB);
                collection.Upsert(folders);
                tcs.SetResult(folders);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public void DeleteAll()
        {
            this.Drop();
        }

        /// <inheritdoc/>
        public void Drop()
        {
            this.db.DropCollection(SettingsDB);
            this.db.DropCollection(VideoDB);
            this.db.DropCollection(MusicDB);
        }

        /// <inheritdoc/>
        public Task<bool> ContainsTrackAsync(string path)
        {
            var tcs = new TaskCompletionSource<bool>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<TrackItem>(MusicDB);
                tcs.SetResult(collection.Exists(n => n.Path == path));
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<List<ArtistItem>> FetchArtistsAsync()
        {
            var tcs = new TaskCompletionSource<List<ArtistItem>>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<ArtistItem>(MusicDB);
                tcs.SetResult(collection.FindAll().ToList());
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<List<AlbumItem>> FetchAlbumsAsync()
        {
            var tcs = new TaskCompletionSource<List<AlbumItem>>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<AlbumItem>(MusicDB);
                tcs.SetResult(collection.FindAll().ToList());
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<List<TrackItem>> FetchTracksAsync()
        {
            var tcs = new TaskCompletionSource<List<TrackItem>>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<TrackItem>(MusicDB);
                tcs.SetResult(collection.FindAll().ToList());
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<ArtistItem?> FetchArtistViaNameAsync(string name)
        {
            var tcs = new TaskCompletionSource<ArtistItem?>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<ArtistItem>(MusicDB);
                tcs.SetResult(collection.FindOne(n => n.Name != null && n.Name.Contains(name)));
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<AlbumItem?> FetchAlbumViaNameAsync(int artistId, string name)
        {
            var tcs = new TaskCompletionSource<AlbumItem?>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<AlbumItem>(MusicDB);
                tcs.SetResult(collection.FindOne(n => n.ArtistItemId == artistId && n.Name != null && n.Name.Contains(name)));
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<ArtistItem> AddArtistAsync(ArtistItem artist)
        {
            var tcs = new TaskCompletionSource<ArtistItem>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<ArtistItem>(MusicDB);
                collection.Upsert(artist);
                tcs.SetResult(artist);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<AlbumItem> AddAlbumAsync(AlbumItem album)
        {
            var tcs = new TaskCompletionSource<AlbumItem>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<AlbumItem>(MusicDB);
                collection.Upsert(album);
                tcs.SetResult(album);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<TrackItem> AddTrackAsync(TrackItem track)
        {
            var tcs = new TaskCompletionSource<TrackItem>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<TrackItem>(MusicDB);
                collection.Upsert(track);
                tcs.SetResult(track);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<ArtistItem> UpdateArtistAsync(ArtistItem artist)
        {
            var tcs = new TaskCompletionSource<ArtistItem>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<ArtistItem>(MusicDB);
                collection.Upsert(artist);
                tcs.SetResult(artist);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<AlbumItem> UpdateAlbumAsync(AlbumItem album)
        {
            var tcs = new TaskCompletionSource<AlbumItem>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<AlbumItem>(MusicDB);
                collection.Upsert(album);
                tcs.SetResult(album);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<TrackItem> UpdateTrackAsync(TrackItem track)
        {
            var tcs = new TaskCompletionSource<TrackItem>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<TrackItem>(MusicDB);
                collection.Upsert(track);
                tcs.SetResult(track);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<AlbumItem?> FetchAlbumViaIdAsync(int id)
        {
            var tcs = new TaskCompletionSource<AlbumItem?>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<AlbumItem>(MusicDB);
                tcs.SetResult(collection.FindOne(n => n.Id == id));
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<AlbumItem?> FetchAlbumWithTracksViaIdAsync(int id)
        {
            var tcs = new TaskCompletionSource<AlbumItem?>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<AlbumItem>(MusicDB);
                tcs.SetResult(collection.FindOne(n => n.Id == id));
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<ArtistItem?> FetchArtistViaIdAsync(int id)
        {
            var tcs = new TaskCompletionSource<ArtistItem?>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<ArtistItem>(MusicDB);
                tcs.SetResult(collection.FindOne(n => n.Id == id));
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<ArtistItem?> FetchArtistWithAlbumsViaIdAsync(int id)
        {
            var tcs = new TaskCompletionSource<ArtistItem?>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<ArtistItem>(MusicDB);
                tcs.SetResult(collection.FindOne(n => n.Id == id));
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<TrackItem?> FetchTrackViaIdAsync(int id)
        {
            var tcs = new TaskCompletionSource<TrackItem?>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<TrackItem>(MusicDB);
                tcs.SetResult(collection.FindOne(n => n.Id == id));
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<ArtistItem> RemoveArtistAsync(ArtistItem artist)
        {
            var tcs = new TaskCompletionSource<ArtistItem>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<ArtistItem>(MusicDB);
                collection.DeleteMany(n => n.Id == artist.Id);
                tcs.SetResult(artist);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<AlbumItem> RemoveAlbumAsync(AlbumItem album)
        {
            var tcs = new TaskCompletionSource<AlbumItem>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<AlbumItem>(MusicDB);
                collection.DeleteMany(n => n.Id == album.Id);
                tcs.SetResult(album);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<TrackItem> RemoveTrackAsync(TrackItem track)
        {
            var tcs = new TaskCompletionSource<TrackItem>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<AlbumItem>(MusicDB);
                collection.DeleteMany(n => n.Id == track.Id);
                tcs.SetResult(track);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<bool> ContainsVideoAsync(string path)
        {
            var tcs = new TaskCompletionSource<bool>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<VideoItem>(VideoDB);
                tcs.SetResult(collection.Exists(n => n.Path == path));
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<VideoItem> AddVideoItemAsync(VideoItem video)
        {
            var tcs = new TaskCompletionSource<VideoItem>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<VideoItem>(VideoDB);
                collection.Upsert(video);
                tcs.SetResult(video);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<VideoItem> UpdateVideoItemAsync(VideoItem video)
        {
            var tcs = new TaskCompletionSource<VideoItem>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<VideoItem>(VideoDB);
                collection.Upsert(video);
                tcs.SetResult(video);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<VideoItem> RemoveVideoItemAsync(VideoItem video)
        {
            var tcs = new TaskCompletionSource<VideoItem>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<VideoItem>(VideoDB);
                collection.DeleteMany(n => n.Id == video.Id);
                tcs.SetResult(video);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<TVShow> AddTVShowAsync(TVShow show)
        {
            var tcs = new TaskCompletionSource<TVShow>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<TVShow>(VideoDB);
                collection.Upsert(show);
                tcs.SetResult(show);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<TVShow> UpdateTVShowAsync(TVShow show)
        {
            var tcs = new TaskCompletionSource<TVShow>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<TVShow>(VideoDB);
                collection.Upsert(show);
                tcs.SetResult(show);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<TVShow> RemoveTVShowAsync(TVShow show)
        {
            var tcs = new TaskCompletionSource<TVShow>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<TVShow>(VideoDB);
                collection.DeleteMany(n => n.Id == show.Id);
                tcs.SetResult(show);
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<TVShow?> FetchTVShowViaNameAsync(string name)
        {
            var tcs = new TaskCompletionSource<TVShow?>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<TVShow>(VideoDB);
                tcs.SetResult(collection.FindOne(n => n.ShowTitle != null && n.ShowTitle.Contains(name)));
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<TVShow?> FetchTVShowWithEpisodesAsync(int id)
        {
            var tcs = new TaskCompletionSource<TVShow?>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<TVShow>(VideoDB);
                tcs.SetResult(collection.FindOne(n => n.Id == id));
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<List<TVShow>> FetchTVShowsAsync()
        {
            var tcs = new TaskCompletionSource<List<TVShow>>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<TVShow>(VideoDB);
                tcs.SetResult(collection.FindAll().ToList());
            });
            return tcs.Task;
        }

        /// <inheritdoc/>
        public Task<List<VideoItem>> FetchVideosAsync()
        {
            var tcs = new TaskCompletionSource<List<VideoItem>>();
            Task.Run(() =>
            {
                var collection = this.db.GetCollection<VideoItem>(VideoDB);
                tcs.SetResult(collection.FindAll().ToList());
            });
            return tcs.Task;
        }
    }
}
