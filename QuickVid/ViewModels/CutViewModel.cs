using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickVid.Behaviours;
using QuickVid.Export.Operations;
using QuickVid.Services;
using QuickVid.Views;
using Unosquare.FFME;
using Unosquare.FFME.Common;

namespace QuickVid.ViewModels;

internal sealed partial class CutViewModel : BaseViewModel, IFileDropHandler
{
    private readonly ExportViewModel _exportService;
    private readonly Queue<FileInfo> _files = new();
    private          FileInfo?       _currentFile;
    private          MediaElement?   _mediaElement;

    [ObservableProperty]
    private TimeSpan _totalMediaDuration;

    [ObservableProperty]
    private double _rangeStart;

    [ObservableProperty]
    private double _rangeEnd;

    public CutViewModel()
    {
        _exportService = ServiceProvider.Instance.GetService<ExportView>().ViewModel;
    }

    public MediaElement MediaElement
    {
        get
        {
            if (_mediaElement == null)
            {
                _mediaElement = ServiceProvider.Instance.GetService<CutView>().MediaElement;
                _mediaElement.PositionChanged += OnPositionChanged;
            }

            return _mediaElement;
        }
    }

    public TimeSpan CurrentMediaPosition
    {
        get => MediaElement?.Position ?? TimeSpan.Zero;
        set => MediaElement.Position = value;
    }

    partial void OnRangeStartChanged(double value)
    {
        var time = TimeSpan.FromSeconds(value);
        if (time > CurrentMediaPosition)
            MediaElement.Position = time;
    }

    private async Task OpenFile(string path)
    {
        if (!File.Exists(path))
            return;
        _currentFile = new FileInfo(path);
        CurrentMediaPosition = TotalMediaDuration = TimeSpan.Zero;
        RangeStart = 0;
        var result = await MediaElement.Open(new Uri(_currentFile.FullName));
        if (!result)
            return;
        TotalMediaDuration = MediaElement.NaturalDuration ?? TimeSpan.Zero;
        RangeEnd = TotalMediaDuration.TotalSeconds;
        await MediaElement.Play();
    }

    [RelayCommand]
    private async Task AddToExportQueue()
    {
        if (_currentFile == null)
            return;

        var saveFileDialog = new SaveFileDialog
        {
                Filter = "MP4|*.mp4",
                FileName = $"{Path.GetFileNameWithoutExtension(_currentFile.Name)}_edited.mp4"
        };

        var result = saveFileDialog.ShowDialog();
        if (result != DialogResult.OK)
            return;
        await MediaElement.Pause();

        var operation = new CutOperation
        {
                File = _currentFile,
                OutputFile = new FileInfo(saveFileDialog.FileName),
                TimeSeek = TimeSpan.FromSeconds(RangeStart),
                OutputTime = TimeSpan.FromSeconds(RangeEnd - RangeStart)
        };

        _exportService.Enqueue(operation);
        //TODO: Add to export queue

        if (_files.TryDequeue(out var file))
            await OpenFile(file.FullName);
    }

    private void OnPositionChanged(object? sender, PositionChangedEventArgs e)
    {
        if (e.Position.TotalSeconds >= RangeEnd)
        {
            MediaElement.Position = TimeSpan.FromSeconds(RangeStart);
        }

        OnPropertyChanged(nameof(CurrentMediaPosition));
    }

    public async void OnFilesDropped(string[] files)
    {
        await OpenFile(files[0]);
        if (files.Length <= 1)
            return;
        for (var i = 1; i < files.Length; i++)
            _files.Enqueue(new FileInfo(files[i]));
    }
}