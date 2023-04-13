using Xabe.FFmpeg;

namespace QuickVid.Export;

public record struct ConversionSettings(IMediaInfo MediaInfo, IConversion Conversion, IVideoStream VideoStream, IAudioStream? AudioStream)
{

}