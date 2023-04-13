using Xabe.FFmpeg;

namespace QuickVid.Export;

public sealed class ExportSettings
{
    public Format OutputFormat { get; set; } = Format.mp4;
    public ConversionPreset ConversionPreset { get; set; } = ConversionPreset.SuperFast;
    public bool UseMultithreading { get; set; } = true;
    public bool RemoveAudio { get; set; } = false;
    public bool UseConstantRateFactor { get; set; } = false;
    public float ConstantRateFactor { get; set; } = 23;
    public double? Framerate { get; set; } = null;
    public long? VideoBitrate { get; set; } = null;
    public long? AudioBitrate { get; set; } = null;
}