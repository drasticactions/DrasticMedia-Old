// <copyright file="App.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DrasticMedia;

/// <summary>
/// Application.
/// </summary>
public partial class App : Application
{
    private IServiceProvider services;

    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    /// <param name="services">IServiceProvider.</param>
    public App(IServiceProvider services)
    {
        this.InitializeComponent();
        this.services = services;
    }

    /// <inheritdoc/>
    protected override Window CreateWindow(IActivationState activationState)
    {
        // return new MediaWindow(this.services) { Page = new HolderPage(new MenuPage(this.services), new NavigationPage(new RecentlyPlayedPage(this.services))) };
        //return new MediaWindow(this.services) { Page = new NavigationPage(new DesktopMusicArtistPage(this.services)) };
        return new MediaWindow(this.services) { Page = new NavigationPage(new DebugPage(this.services)) };
    }
}
