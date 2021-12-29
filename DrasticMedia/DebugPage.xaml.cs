// <copyright file="DebugPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Library;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace DrasticMedia
{
    public partial class DebugPage : BasePage
    {
        private MediaLibrary mediaLibrary;

        public DebugPage(IServiceProvider services)
            : base(services)
        {
            this.InitializeComponent();
            this.mediaLibrary = this.Services.GetService<MediaLibrary>();

        }

        private async void OnMusicLibrarySync(object sender, EventArgs e)
        {
            await this.mediaLibrary.ScanMediaDirectoriesAsync(@"C:\Users\t_mil\Music");
        }
    }
}
