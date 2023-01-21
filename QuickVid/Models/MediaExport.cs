using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using QuickVid.Export;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Events;

namespace QuickVid.Models;

public sealed partial class MediaExport : ObservableObject
{
    public static readonly IReadOnlyList<ConversionPreset> ConversionPresets;

    [ObservableProperty]
    private bool _isExporting;

    [ObservableProperty]
    private int _progressPercentage;

    static MediaExport()
    {
        ConversionPresets = Enum.GetValues<ConversionPreset>().ToImmutableList();
    }

    public MediaExport(ExportOperation operation)
    {
        Operation = operation;
    }

    public ExportOperation                 Operation { get; }
    public IReadOnlyList<ConversionPreset> Presets   => ConversionPresets;

    public ConversionPreset ConversionPreset
    {
        get => Operation.Settings.ConversionPreset;
        set => Operation.Settings.ConversionPreset = value;
    }

    public bool RemoveAudio
    {
        get => Operation.Settings.RemoveAudio;
        set => Operation.Settings.RemoveAudio = value;
    }

    public async Task<IConversionResult> Export()
    {
        IsExporting = true;
        var result = await Operation.Convert(OnExportProgressionChanged);
        IsExporting = false;
        return result;
    }

    private void OnExportProgressionChanged(object? sender, ConversionProgressEventArgs e)
    {
        ProgressPercentage = e.Percent;
    }
}