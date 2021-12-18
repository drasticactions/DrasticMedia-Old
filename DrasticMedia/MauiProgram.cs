// <copyright file="MauiProgram.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core;
using DrasticMedia.Core.Services;
using DrasticMedia.Services;

namespace DrasticMedia;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        var libvlc = new LibVLCSharp.Shared.LibVLC();
        var mediaplayer = new LibVLCSharp.Shared.MediaPlayer(libvlc);
        var mediaService = new VLCMediaService(mediaplayer, libvlc);
        var consoleLogger = new ConsoleLogger();
        builder.Services.AddSingleton(libvlc);
        builder.Services.AddSingleton(mediaplayer);
        builder.Services.AddSingleton<ILogger>(consoleLogger);
        builder.Services.AddSingleton<IMediaService>(mediaService);
        builder.Services.AddSingleton<IWindowTappedService, WindowTappedService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
        builder.Services.AddSingleton<PlayerService>();
        builder
          .UseMauiApp<App>()
          .ConfigureFonts(fonts =>
          {
              fonts.AddFont("FontAwesome6Brands-Regular-400.otf", "FontAwesomeBrands");
              fonts.AddFont("FontAwesome6Free-Regular-400.otf", "FontAwesomeRegular");
              fonts.AddFont("FontAwesome6Free-Solid-900.otf", "FontAwesomeSolid");
              fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
          });

        return builder.Build();
    }
}
