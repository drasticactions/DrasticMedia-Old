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

        public Model.MediaItem? CurrentMedia { get { return media; } set { this.media = value; this.SetCurrentMedia(); } }

        public bool IsPlaying => this.mediaPlayer.IsPlaying;

        public double CurrentPosition => this.mediaPlayer.Position;

        public VLCMediaService(VLCPlayer player, LibVLC libVLC)
        {
            this.mediaPlayer = player;
            this.libVLC = libVLC;
        }

        public Task PauseAsync()
        {
            this.mediaPlayer.Pause();
            return Task.CompletedTask;
        }

        public Task PlayAsync(double position = 0, bool fromPosition = false)
        {
            this.mediaPlayer.Play();
            return Task.CompletedTask;
        }

        public Task ResumeAsync()
        {
            this.mediaPlayer.Play();
            return Task.CompletedTask;
        }

        public Task SkipAhead(double amount = 0)
        {
            return Task.CompletedTask;
        }

        public Task SkipBack(double amount = 0)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            this.mediaPlayer.Stop();
            return Task.CompletedTask;
        }

        public Task<string> GetArtworkUrl() => this.GetMetadata(MetadataType.ArtworkURL);

        private void SetCurrentMedia()
        {
            if (this.CurrentMedia?.Path == null)
            {
                throw new NullReferenceException(nameof(this.CurrentMedia));
            }

            this.mediaPlayer.Stop();
            this.vlcMedia = new LibVLCSharp.Shared.Media(this.libVLC, this.CurrentMedia.Path);
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
    }
}
