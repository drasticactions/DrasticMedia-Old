// <copyright file="MediaHelpers.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core.Exceptions;
using DrasticMedia.Core.Model;
using LibVLCSharp.Shared;
using System.Web;
using static DrasticMedia.Core.FileExtensions;

namespace DrasticMedia.Core.Helpers
{
    /// <summary>
    /// Media Helpers.
    /// </summary>
    public static class MediaHelpers
    {
        /// <summary>
        /// Gets video properties for a given storage file.
        /// </summary>
        /// <param name="libVLC">LibVLC Instance.</param>
        /// <param name="path">Item to be parsed.</param>
        /// <param name="type">Item type.</param>
        /// <param name="fileType">Type of file.</param>
        /// <returns>MediaProperties.</returns>
        /// <exception cref="ParseMediaException">Thrown is media fails to parse or is unsupported.</exception>
        public static Task<IMediaItem> GetMediaPropertiesAsync(this LibVLC libVLC, string path, FromType type = FromType.FromPath, MediaFileType fileType = MediaFileType.Unknown)
        {
            if (fileType == MediaFileType.Unknown)
            {
                fileType = FileExtensions.GetFileType(path);
            }

            switch (fileType)
            {
                case MediaFileType.Audio:
                    return libVLC.GetMusicPropertiesAsync(path, type);
                case MediaFileType.Video:
                    return libVLC.GetVideoPropertiesAsync(path, type);
                default:
                    throw new ParseMediaException($"Failed to parse {path}, Media Unknown");
            }
        }

        /// <summary>
        /// Gets video properties for a given storage file.
        /// </summary>
        /// <param name="libVLC">LibVLC Instance.</param>
        /// <param name="path">Item to be parsed.</param>
        /// <param name="type">Item type.</param>
        /// <returns>Music MediaProperties.</returns>
        public static Task<IMediaItem> GetVideoPropertiesAsync(this LibVLC libVLC, string path, FromType type = FromType.FromPath)
        {
            return GetVideoPropertiesAsync(libVLC, new VideoItem() { Path = path }, type);
        }

        /// <summary>
        /// Gets video properties for a given storage file.
        /// </summary>
        /// <param name="libVLC">LibVLC Instance.</param>
        /// <param name="mP">Item to be parsed.</param>
        /// <param name="type">Item type.</param>
        /// <returns>Music MediaProperties.</returns>
        public static async Task<IMediaItem> GetVideoPropertiesAsync(this LibVLC libVLC, VideoItem mP, FromType type)
        {
            if (mP.Path is null)
            {
                throw new ParseMediaException($"Could not parse media path.");
            }

            var media = new LibVLCSharp.Shared.Media(libVLC, mP.Path, type);
            var parseStatus = await media.Parse(MediaParseOptions.ParseLocal & MediaParseOptions.FetchLocal).ConfigureAwait(false);
            if (parseStatus == MediaParsedStatus.Failed)
            {
                throw new ParseMediaException($"Could not parse {mP.Path}");
            }

            mP.Title = media.Meta(MetadataType.Title);

            var showName = media.Meta(MetadataType.ShowName);
            if (string.IsNullOrEmpty(showName))
            {
                showName = media.Meta(MetadataType.Artist);
            }

            if (!string.IsNullOrEmpty(showName))
            {
                mP.ShowTitle = showName;
            }

            var episodeString = media.Meta(MetadataType.Episode);
            if (string.IsNullOrEmpty(episodeString))
            {
                episodeString = media.Meta(MetadataType.TrackNumber);
            }

            var episode = 0;
            if (!string.IsNullOrEmpty(episodeString) && int.TryParse(episodeString, out episode))
            {
                mP.Episode = episode;
            }

            var episodesTotal = 0;
            var episodesTotalString = media.Meta(MetadataType.TrackTotal);
            if (!string.IsNullOrEmpty(episodesTotalString) && int.TryParse(episodesTotalString, out episodesTotal))
            {
                mP.Episodes = episodesTotal;
            }

            var videoTrack = media.Tracks.FirstOrDefault(x => x.TrackType == TrackType.Video);
            mP.Width = videoTrack.Data.Video.Width;
            mP.Height = videoTrack.Data.Video.Height;

            var durationLong = media.Duration;
            var duration = TimeSpan.FromMilliseconds(durationLong);
            mP.Duration = duration;

            return mP;
        }

        /// <summary>
        /// Gets music properties for a given storage file.
        /// </summary>
        /// <param name="libVLC">LibVLC Instance.</param>
        /// <param name="mP">Item to be parsed.</param>
        /// <param name="type">Item type.</param>
        /// <returns>Music MediaProperties.</returns>
        public static async Task<IMediaItem> GetMusicPropertiesAsync(this LibVLC libVLC, TrackItem mP, FromType type)
        {
            if (mP.Path is null)
            {
                throw new ParseMediaException($"Could not parse media path.");
            }

            var media = new LibVLCSharp.Shared.Media(libVLC, mP.Path, type);

            var parseStatus = await media.Parse(MediaParseOptions.ParseLocal & MediaParseOptions.FetchLocal).ConfigureAwait(false);
            if (parseStatus == MediaParsedStatus.Failed)
            {
                throw new ParseMediaException($"Could not parse {mP.Path}");
            }

            mP.AlbumArtist = media.Meta(MetadataType.AlbumArtist);
            mP.Artist = media.Meta(MetadataType.Artist);
            mP.Album = media.Meta(MetadataType.Album);
            mP.Title = media.Meta(MetadataType.Title);
            mP.AlbumArt = media.Meta(MetadataType.ArtworkURL);

            if (!string.IsNullOrEmpty(mP.AlbumArt) && type == FromType.FromPath)
            {
                mP.AlbumArt = HttpUtility.UrlDecode((new Uri(mP.AlbumArt).LocalPath));
            }

            var yearString = media.Meta(MetadataType.Date);
            var year = 0;
            if (int.TryParse(yearString, out year))
            {
                mP.Year = year;
            }

            var durationLong = media.Duration;
            TimeSpan duration = TimeSpan.FromMilliseconds(durationLong);
            mP.Duration = duration;

            var trackNbString = media.Meta(MetadataType.TrackNumber);
            uint trackNbInt = 0;
            uint.TryParse(trackNbString, out trackNbInt);
            mP.Tracknumber = trackNbInt;

            var discNb = media.Meta(MetadataType.DiscNumber);
            if (discNb != null && discNb.Contains("/"))
            {
                // if discNb = "1/2"
                var discNumDen = discNb.Split('/');
                if (discNumDen.Any())
                {
                    discNb = discNumDen[0];
                }
            }

            int.TryParse(discNb, out int discNbInt);
            mP.DiscNumber = discNbInt;

            var genre = media.Meta(MetadataType.Genre);
            mP.Genre = genre;
            return mP;
        }

        /// <summary>
        /// Gets music properties for a given storage file.
        /// </summary>
        /// <param name="libVLC">LibVLC Instance.</param>
        /// <param name="path">Item to be parsed.</param>
        /// <param name="type">Item type.</param>
        /// <returns>Music MediaProperties.</returns>
        public static async Task<IMediaItem> GetMusicPropertiesAsync(this LibVLC libVLC, string path, FromType type = FromType.FromPath)
        {
            return await GetMusicPropertiesAsync(libVLC, new TrackItem() { Path = path }, type).ConfigureAwait(false);
        }
    }
}
