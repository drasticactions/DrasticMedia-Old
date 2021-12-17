// <copyright file="AlbumArtPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Services;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace DrasticMedia
{
    public partial class AlbumArtPage : ContentPage
    {
        private PlayerService player;

        public AlbumArtPage(IServiceProvider provider)
        {
            InitializeComponent();
            this.player = provider.GetService<PlayerService>();
            this.BindingContext = this.player;
        }
    }
}
