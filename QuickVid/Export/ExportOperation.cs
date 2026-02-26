using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Events;

namespace QuickVid.Export;

public abstract class ExportOperation
{
    public required FileInfo File { get; init; }
    public required FileInfo OutputFile { get; init; }
    public ExportSettings Settings { get; } = new();

    public abstract Task<IConversionResult> Convert(ConversionProgressEventHandler? progressCallback = null);

    protected virtual async Task<ConversionSettings> BuildDefaultConversion(ConversionProgressEventHandler? progressCallback)
    {
        var mediaInfo = await Xabe.FFmpeg.FFmpeg.GetMediaInfo(File.FullName);
        var conversion = Xabe.FFmpeg.FFmpeg.Conversions.New();
        var videoStream = Settings.OutputFormat == Format.mp3 ? null : mediaInfo.VideoStreams.First();
        var audioStream = Settings.RemoveAudio ? null : mediaInfo.AudioStreams.First();

        conversion.SetOutput($"{OutputFile.FullName}.{Settings.OutputFormat}");
        conversion.SetOutputFormat(Settings.OutputFormat);

        if (videoStream != null)
        {
            conversion.SetFrameRate(Settings.Framerate ?? videoStream.Framerate);
            conversion.SetVideoBitrate(Settings.VideoBitrate ?? videoStream.Bitrate);
            conversion.AddStream(videoStream);
        }

        if (audioStream != null)
        {
            conversion.SetAudioBitrate(Settings.AudioBitrate ?? audioStream.Bitrate);
            conversion.AddStream(audioStream);
        }

        conversion.UseMultiThread(Settings.UseMultithreading);
        conversion.SetPreset(Settings.ConversionPreset);
        if (Settings is { UseConstantRateFactor: true, ConstantRateFactor: >= 0 and <= 51 })
        {
            // Discord doesn't actually support h265 :'(
            //conversion.AddParameter("-vcodec libx265"); // Use libx265, better compression without quality loss but conversion is slower
            conversion.AddParameter($"-crf {Settings.ConstantRateFactor}");
        }

        if (progressCallback != null)
            conversion.OnProgress += progressCallback;

        return new(mediaInfo, conversion, videoStream, audioStream);
    }
}