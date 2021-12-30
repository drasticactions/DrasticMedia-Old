// <copyright file="AlbumPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrasticMedia.Core.Model;
using DrasticMedia.Core.Utilities;
using DrasticMedia.Utilities;
using DrasticMedia.ViewModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace DrasticMedia
{
    public partial class AlbumPage : BasePage
    {
        public AlbumPage(IServiceProvider services, int albumId)
            : base(services)
        {
            this.InitializeComponent();
            this.ViewModel = services.ResolveWith<AlbumPageViewModel>(this, albumId);
            this.BindingContext = this.ViewModel;
        }
    }
}
