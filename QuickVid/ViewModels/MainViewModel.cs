using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using Xabe.FFmpeg.Downloader;

namespace QuickVid.ViewModels;

internal sealed partial class MainViewModel : BaseViewModel
{
    public static readonly string FFmpegDirectory = Path.Combine(AppContext.BaseDirectory, "ffmpeg");

    [ObservableProperty]
    private bool _allowNavigation = false;

    public MainViewModel()
    {
        CheckFFmpeg();
    }

    private async void CheckFFmpeg()
    {
        if (!Directory.Exists(FFmpegDirectory))
        {
            Directory.CreateDirectory(FFmpegDirectory);
            await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Shared, FFmpegDirectory);
        }

        Xabe.FFmpeg.FFmpeg.SetExecutablesPath(FFmpegDirectory);
        Unosquare.FFME.Library.FFmpegDirectory = FFmpegDirectory;

        AllowNavigation = true;
    }
}