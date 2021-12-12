// <copyright file="MauiProgram.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMedia.Core;
using DrasticMedia.Core.Services;

namespace DrasticMedia;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        var libvlc = new LibVLCSharp.Shared.LibVLC();
        var mediaplayer = new LibVLCSharp.Shared.MediaPlayer(libvlc);
        builder.Services.AddSingleton(libvlc);
        builder.Services.AddSingleton(mediaplayer);
        builder.Services.AddSingleton<ILogger, ConsoleLogger>();
        builder.Services.AddSingleton<IMediaService>(new VLCMediaService(mediaplayer, libvlc));
        builder.Services.AddSingleton<PlayerService>();
        builder
          .UseMauiApp<App>()
          .ConfigureFonts(fonts =>
          {
              fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
          });

        return builder.Build();
    }
}
