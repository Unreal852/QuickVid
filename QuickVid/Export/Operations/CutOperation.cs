using System;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Events;

namespace QuickVid.Export.Operations;

internal sealed class CutOperation : ExportOperation
{
    public required TimeSpan TimeSeek   { get; init; }
    public required TimeSpan OutputTime { get; init; }

    public override async Task<IConversionResult> Convert(ConversionProgressEventHandler? progressCallback = null)
    {
        var conv = await BuildDefaultConversion(progressCallback);

        conv.Conversion.SetSeek(TimeSeek);
        conv.Conversion.SetOutputTime(OutputTime);

        return await conv.Conversion.Start();
    }
}