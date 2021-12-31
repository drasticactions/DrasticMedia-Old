// <copyright file="App.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Maui.Platform;

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

        Microsoft.Maui.Handlers.LayoutHandler.LayoutMapper.AppendToMapping(nameof(IView.Background), (handler, view) =>
        {
            if (view is PlayerGrid playerGrid)
            {
#if WINDOWS
                var grid = playerGrid.Handler.GetWrappedNativeView() as LayoutPanel;
                grid.Background = MauiWinUIApplication.Current.Resources["SystemControlAcrylicElementBrush"] as Microsoft.UI.Xaml.Media.Brush;
#endif
            }
        });
    }

    /// <inheritdoc/>
    protected override Window CreateWindow(IActivationState activationState)
    {
        // return new MediaWindow(this.services) { Page = new HolderPage(new MenuPage(this.services), new NavigationPage(new RecentlyPlayedPage(this.services))) };
        return new MediaWindow(this.services) { Page = new NavigationPage(new DesktopMusicArtistPage(this.services)) };
        //return new MediaWindow(this.services) { Page = new NavigationPage(new DebugPage(this.services)) };
    }
}
