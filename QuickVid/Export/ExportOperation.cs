using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Events;

namespace QuickVid.Export;

public abstract class ExportOperation
{
    public required FileInfo       File       { get; init; }
    public required FileInfo       OutputFile { get; init; }
    public          ExportSettings Settings   { get; } = new();

    public abstract Task<IConversionResult> Convert(ConversionProgressEventHandler? progressCallback = null);

    protected virtual async
            Task<(IMediaInfo MediaInfo, IConversion Conversion, IVideoStream VideoStream, IAudioStream? AudioStream)>
            BuildDefaultConversion(ConversionProgressEventHandler? progressCallback)
    {
        var mediaInfo = await Xabe.FFmpeg.FFmpeg.GetMediaInfo(File.FullName);
        var conversion = Xabe.FFmpeg.FFmpeg.Conversions.New();
        var videoStream = mediaInfo.VideoStreams.First();
        var audioStream = Settings.RemoveAudio ? default : mediaInfo.AudioStreams.First();

        conversion.SetOutput(OutputFile.FullName);
        conversion.SetOutputFormat(Settings.OutputFormat);
        conversion.SetFrameRate(Settings.Framerate       ?? videoStream.Framerate);
        conversion.SetVideoBitrate(Settings.VideoBitrate ?? videoStream.Bitrate);
        conversion.AddStream(videoStream);

        if (audioStream != null)
        {
            conversion.SetAudioBitrate(Settings.AudioBitrate ?? audioStream.Bitrate);
            conversion.AddStream(audioStream);
        }

        conversion.UseMultiThread(Settings.UseMultithreading);
        conversion.SetPreset(Settings.ConversionPreset);

        if (progressCallback != null)
            conversion.OnProgress += progressCallback;

        return (mediaInfo, conversion, videoStream, audioStream);
    }
}