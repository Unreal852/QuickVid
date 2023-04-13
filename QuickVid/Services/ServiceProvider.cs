using Jab;
using QuickVid.Views;
using Serilog;

namespace QuickVid.Services;

[ServiceProvider]
[Singleton<ILogger>(Instance = nameof(Logger))]

// Views
[Singleton<CutView>]
[Singleton<MergeView>]
[Singleton<ExportView>]
public sealed partial class ServiceProvider
{
    public static readonly ServiceProvider Instance;
    public static          ILogger         Logger => Log.Logger;

    static ServiceProvider()
    {
        Instance = new ServiceProvider();
    }
}