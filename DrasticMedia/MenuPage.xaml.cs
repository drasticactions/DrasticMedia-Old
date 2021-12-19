// <copyright file="MenuPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace DrasticMedia
{
    public partial class MenuPage : BasePage
    {
        public MenuPage(IServiceProvider provider)
           : base(provider)
        {
            this.InitializeComponent();
        }
    }
}
