// <copyright file="MauiProgram.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DrasticMaui.Services;
using DrasticMedia.Core;
using DrasticMedia.Core.Database;
using DrasticMedia.Core.Library;
using DrasticMedia.Core.Metadata;
using DrasticMedia.Core.Platform;
using DrasticMedia.Core.Services;
using DrasticMedia.Services;
using DrasticMedia.SQLite.Database;
using DrasticMedia.ViewModels;
using DrasticMedia.VLC;
using DrasticMedia.VLC.Library;
using ReorderableCollectionView.Maui;

namespace DrasticMedia;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        IMediaService service = null;
#if IOS || MACCATALYST
        SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_sqlite3());
#endif

#if ANDROID
        service = new NativeMediaService(MainActivity.instance as DrasticMedia.Native.Activity.IMediaActivity);
#else
        service = new NativeMediaService();
#endif

        var builder = MauiApp.CreateBuilder();

#if WINDOWS || ANDROID
        LibVLCSharp.Shared.Core.Initialize();
        var libvlc = new LibVLCSharp.Shared.LibVLC();
        var mediaplayer = new LibVLCSharp.Shared.MediaPlayer(libvlc);
        var mediaService = new VLCMediaService(mediaplayer, libvlc);
        builder.Services.AddSingleton(libvlc);
        builder.Services.AddSingleton(mediaplayer);
        builder.Services.AddSingleton<ILocalMetadataParser, VLCMediaParser>();
        builder.Services.AddSingleton<IMediaService>(mediaService);
#else
        builder.Services.AddSingleton<ILocalMetadataParser, FFMpegMediaParser>();
        builder.Services.AddSingleton<IMediaService>(service);
#endif
        var consoleLogger = new ConsoleLogger();

        builder.Services.AddSingleton<IPlatformSettings, PlatformSettings>();
        builder.Services.AddSingleton<IMetadataService, LastfmMetadataService>();
        builder.Services.AddSingleton<IMetadataService, SpotifyMetadataService>();
        builder.Services.AddSingleton<IPodcastDatabase, PodcastDatabase>();
        builder.Services.AddSingleton<IMusicDatabase, MusicDatabase>();
        builder.Services.AddSingleton<IVideoDatabase, VideoDatabase>();

        builder.Services.AddSingleton<MediaLibrary>();
        builder.Services.AddSingleton<ILogger>(consoleLogger);
        builder.Services.AddSingleton<IWindowTappedService, WindowTappedService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
        builder.Services.AddSingleton<PlayerService>();
        //builder.Services.AddSingleton<PlayerPageViewModel>();
        builder.Services.AddTransient<PodcastListPageViewModel>();
        builder.Services.AddTransient<PodcastEpisodeListPageViewModel>();
        builder.Services.AddTransient<ArtistListPageViewModel>();
        builder.Services.AddTransient<AlbumListPageViewModel>();
        builder.Services.AddTransient<AlbumPageViewModel>();
        //builder.Services.AddTransient<PlayerPage>();
        builder.Services.AddTransient<PodcastListPage>();
        builder.Services.AddTransient<DebugPage>();
        builder.Services.AddTransient<AlbumPage>();
        builder.Services.AddTransient<DesktopMusicArtistPage>();
        builder.Services.AddTransient<DesktopPodcastPage>();
        //builder.Services.AddTransient<PodcastEpisodeListPage>();

        builder
          .UseMauiApp<DrasticMedia.App>()
          .RegisterReorderableCollectionView()
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
