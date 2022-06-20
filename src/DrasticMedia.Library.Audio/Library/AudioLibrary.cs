// <copyright file="AudioLibrary.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core;
using DrasticMedia.Core.Database;
using DrasticMedia.Core.Library;
using DrasticMedia.Core.Metadata;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Platform;
using DrasticMedia.Core.Tools;

namespace DrasticMedia.Audio.Library
{
    public class AudioLibrary : MediaLibrary, IAudioLibrary
    {
        private IMusicDatabase musicDatabase;
        private IPlatformSettings platform;
        private ILogger? logger;
        private IEnumerable<IAudioMetadataService> metadataServices;
        private ILocalMetadataParser mediaParser;

        public AudioLibrary(ILocalMetadataParser mediaParser, IMusicDatabase database, IPlatformSettings platform, IEnumerable<IAudioMetadataService>? metadataServices, ILogger? logger = null)
        {
            this.mediaParser = mediaParser;
            this.musicDatabase = database;
            this.logger = logger;
            this.platform = platform;
            this.metadataServices = metadataServices ?? new List<IAudioMetadataService>();
        }

        /// <inheritdoc/>
        public async Task RemoveArtistAsync(ArtistItem artist)
        {
            artist = await this.musicDatabase.RemoveArtistAsync(artist).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(artist));
        }

        /// <inheritdoc/>
        public async Task RemoveAlbumAsync(AlbumItem album)
        {
            album = await this.musicDatabase.RemoveAlbumAsync(album).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(album));
            if (album.ArtistItem?.Albums != null && album.ArtistItem.Albums.Count <= 0)
            {
                await this.RemoveArtistAsync(album.ArtistItem).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task RemoveTrackAsync(TrackItem track)
        {
            track = await this.musicDatabase.RemoveTrackAsync(track).ConfigureAwait(false);
            this.OnRemoveMediaItem(new RemoveMediaItemEventArgs(track));
            if (track.AlbumItem?.Tracks != null && track.AlbumItem.Tracks.Count <= 0)
            {
                await this.RemoveAlbumAsync(track.AlbumItem).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task<AlbumItem?> FetchAlbumWithTracksViaIdAsync(int id) => await this.musicDatabase.FetchAlbumWithTracksViaIdAsync(id);

        /// <inheritdoc/>
        public async Task<List<ArtistItem>> FetchArtistsAsync() => await this.musicDatabase.FetchArtistsAsync();

        /// <inheritdoc/>
        public async Task<ArtistItem?> FetchArtistViaIdAsync(int id) => await this.musicDatabase.FetchArtistViaIdAsync(id);

        /// <inheritdoc/>
        public async Task<ArtistItem?> FetchArtistWithAlbumsViaIdAsync(int id) => await this.musicDatabase.FetchArtistWithAlbumsViaIdAsync(id);

        /// <inheritdoc/>
        public async Task<ArtistItem?> FetchArtistViaNameAsync(string name) => await this.musicDatabase.FetchArtistViaNameAsync(name);

        /// <inheritdoc/>
        public async Task<List<AlbumItem>> FetchAlbumsAsync() => await this.musicDatabase.FetchAlbumsAsync();

        /// <inheritdoc/>
        public async Task<AlbumItem?> FetchAlbumViaIdAsync(int id) => await this.musicDatabase.FetchAlbumViaIdAsync(id);

        /// <inheritdoc/>
        public async Task<TrackItem?> FetchTrackViaIdAsync(int id) => await this.musicDatabase.FetchTrackViaIdAsync(id);

        /// <inheritdoc/>
        public async Task<List<TrackItem>> FetchTracksAsync() => await this.musicDatabase.FetchTracksAsync();

        /// <inheritdoc/>
        public override async Task<bool> AddFileAsync(string path)
        {
            try
            {
                if (!this.platform.IsFileAvailable(path))
                {
                    return false;
                }

                var fileType = Path.GetExtension(path);
                if (!FileExtensions.AudioExtensions.Contains(fileType))
                {
                    return false;
                }

                var trackInDb = await this.musicDatabase.ContainsTrackAsync(path).ConfigureAwait(false);
                if (trackInDb)
                {
                    // Already in DB, return.
                    return true;
                }

                var mP = await this.mediaParser.GetMusicPropertiesAsync(path) as TrackItem;
                if (mP is null || (string.IsNullOrEmpty(mP.Artist) && string.IsNullOrEmpty(mP.Album) && string.IsNullOrEmpty(mP.Title)))
                {
                    // We couldn't parse the file or the metadata is empty. Skip it.
                    this.OnNewMediaItemError(new NewMediaItemErrorEventArgs() { MediaItemPath = path });
                    return false;
                }

                var artistName = mP.Artist?.Trim();
                if (artistName == null)
                {
                    return false;
                }

                var artist = await this.musicDatabase.FetchArtistViaNameAsync(artistName).ConfigureAwait(false);
                if (artist is null)
                {
                    artist = new ArtistItem()
                    {
                        Name = artistName,
                    };
                    artist = await this.musicDatabase.AddArtistAsync(artist);

                    if (this.metadataServices.Any())
                    {
                        foreach (var service in this.metadataServices)
                        {
                            await this.UpdateArtistMetadata(service, artist);
                        }
                    }

                    var artistImage = artist.ArtistUrlFromMetadata();

                    if (artistImage is not null)
                    {
                        artist.ArtistImage = await this.mediaParser.CacheArtistImageToStorage(artist, artistImage);
                    }

                    artist = await this.musicDatabase.UpdateArtistAsync(artist);

                    this.OnNewMediaItemAdded(new NewMediaItemEventArgs(artist));
                }

                var albumName = mP.Album?.Trim();
                if (albumName != null)
                {
                    var album = await this.musicDatabase.FetchAlbumViaNameAsync(artist.Id, albumName);
                    if (album is null)
                    {
                        album = new AlbumItem()
                        {
                            ArtistItemId = artist.Id,
                            Name = albumName,
                            Year = mP.Year,
                        };
                        album.AlbumArt = mP.AlbumArt;
                        album = await this.musicDatabase.AddAlbumAsync(album);

                        if (this.metadataServices.Any())
                        {
                            foreach (var service in this.metadataServices)
                            {
                                try
                                {
                                    await this.UpdateAlbumMetadata(service, artist, album);
                                }
                                catch (Exception ex)
                                {
                                    this.logger?.Log(ex);
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(album.AlbumArt) && album.AlbumUrlFromMetadata() is not null)
                        {
                            var image = album.AlbumUrlFromMetadata() ?? string.Empty;
                            if (!string.IsNullOrEmpty(image))
                            {
                                album.AlbumArt = await this.mediaParser.CacheAlbumImageToStorage(artist, album, image);
                                album = await this.musicDatabase.UpdateAlbumAsync(album);
                            }
                        }

                        this.OnNewMediaItemAdded(new NewMediaItemEventArgs(album));
                    }

                    mP.AlbumItemId = album.Id;
                    mP.ArtistItemId = artist.Id;
                }

                mP = await this.musicDatabase.AddTrackAsync(mP).ConfigureAwait(false);
                this.OnNewMediaItemAdded(new NewMediaItemEventArgs(mP));
                return mP.Id > 0;
            }
            catch (Exception ex)
            {
                this.OnNewMediaItemError(new NewMediaItemErrorEventArgs() { Exception = ex, MediaItemPath = path });
                this.logger?.Log(ex);
            }

            return false;
        }

        private async Task UpdateArtistMetadata(IAudioMetadataService service, ArtistItem artist)
        {
            var metadata = await service.GetArtistMetadataAsync(artist);
            if (metadata is not null)
            {
                var existingMetadata = artist.Metadata.FirstOrDefault(n => n.Type == metadata.Type);
                if (existingMetadata is not null)
                {
                    metadata.Id = existingMetadata.Id;
                    await this.musicDatabase.UpdateArtistMetadataAsync(metadata);
                }
                else if (metadata is not null)
                {
                    await this.musicDatabase.AddArtistMetadataAsync(metadata);
                    artist.Metadata.Add(metadata);
                }
            }
        }

        private async Task UpdateAlbumMetadata(IAudioMetadataService service, ArtistItem artist, AlbumItem album)
        {
            var metadata = await service.GetAlbumMetadataAsync(album, artist.Name);
            if (metadata is not null)
            {
                var existingMetadata = album.Metadata.FirstOrDefault(n => n.Type == metadata.Type);
                if (existingMetadata is not null)
                {
                    metadata.Id = existingMetadata.Id;
                    await this.musicDatabase.UpdateAlbumMetadataAsync(metadata);
                }
                else if (metadata is not null)
                {
                    await this.musicDatabase.AddAlbumMetadataAsync(metadata);
                    album.Metadata.Add(metadata);
                }
            }
        }
    }
}
