// <copyright file="VLCMediaService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DrasticMedia.Core.Model;
using LibVLCSharp.Shared;
using VLCPlayer = LibVLCSharp.Shared.MediaPlayer;

namespace DrasticMedia.Core.Services
{
    /// <summary>
    /// VLC Media Service.
    /// </summary>
    public class VLCMediaService : IMediaService
    {
        private VLCPlayer mediaPlayer;
        private Model.MediaItem? media;
        private LibVLCSharp.Shared.Media? vlcMedia;
        private LibVLC libVLC;

        /// <summary>
        /// Initializes a new instance of the <see cref="VLCMediaService"/> class.
        /// </summary>
        /// <param name="player">MediaPlayer.</param>
        /// <param name="libVLC">LibVLC.</param>
        public VLCMediaService(VLCPlayer player, LibVLC libVLC)
        {
            this.mediaPlayer = player;
            this.libVLC = libVLC;
            this.mediaPlayer.PositionChanged += this.MediaPlayer_PositionChanged;
        }

        /// <inheritdoc/>
        public event EventHandler<MediaPlayerPositionChangedEventArgs>? PositionChanged;

        /// <inheritdoc/>
        public Model.MediaItem? CurrentMedia { get { return media; } set { this.media = value; this.SetCurrentMedia(); } }

        /// <inheritdoc/>
        public bool IsPlaying => this.mediaPlayer.IsPlaying;

        /// <inheritdoc/>
        public float CurrentPosition { get { return this.mediaPlayer.Position; } set { this.mediaPlayer.Position = (float)value; } }

        /// <inheritdoc/>
        public Task PauseAsync()
        {
            this.mediaPlayer.Pause();
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task PlayAsync(double position = 0, bool fromPosition = false)
        {
            this.mediaPlayer.Play();
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task ResumeAsync()
        {
            this.mediaPlayer.Play();
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task SkipAhead(double amount = 0)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task SkipBack(double amount = 0)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task StopAsync()
        {
            this.mediaPlayer.Stop();
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<string> GetArtworkUrl() => this.GetMetadata(MetadataType.ArtworkURL);

        private void MediaPlayer_PositionChanged(object? sender, LibVLCSharp.Shared.MediaPlayerPositionChangedEventArgs e) 
            => this.PositionChanged?.Invoke(this, new MediaPlayerPositionChangedEventArgs(e.Position));

        private void SetCurrentMedia()
        {
            if (this.CurrentMedia?.Path == null)
            {
                throw new NullReferenceException(nameof(this.CurrentMedia));
            }

            this.mediaPlayer.Stop();
            if (this.PathIsUrl(this.CurrentMedia.Path))
            {
                this.vlcMedia = new LibVLCSharp.Shared.Media(this.libVLC, this.CurrentMedia.Path, FromType.FromLocation);
            }
            else
            {
                this.vlcMedia = new LibVLCSharp.Shared.Media(this.libVLC, this.CurrentMedia.Path);
            }
            this.mediaPlayer.Media = this.vlcMedia;
        }

        private async Task<string> GetMetadata(MetadataType meta)
        {
            if (this.vlcMedia == null)
            {
                throw new ArgumentNullException(nameof(this.vlcMedia));
            }

            if (!this.vlcMedia.IsParsed)
            {
                await this.vlcMedia.Parse();
            }

            var stringUri = this.vlcMedia.Meta(meta);
            if (stringUri == null)
            {
                return string.Empty;
            }

            var uri = new Uri(stringUri);

            return HttpUtility.UrlDecode(uri.LocalPath) ?? string.Empty;
        }

        private bool PathIsUrl(string path)
        {
            if (File.Exists(path))
                return false;
            try
            {
                Uri uri = new Uri(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
