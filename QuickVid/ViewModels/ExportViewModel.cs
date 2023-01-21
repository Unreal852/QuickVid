using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using QuickVid.Export;
using QuickVid.Models;

namespace QuickVid.ViewModels;

public sealed partial class ExportViewModel : BaseViewModel
{
    public ObservableCollection<MediaExport> ExportQueue { get; } = new();

    public void Enqueue(ExportOperation operation)
    {
        ExportQueue.Add(new MediaExport(operation));
    }

    private MediaExport? Dequeue()
    {
        if (ExportQueue.Count > 0)
        {
            var first = ExportQueue[0];
            return first;
        }

        return default;
    }

    [RelayCommand]
    private async Task StartExport()
    {
        var export = Dequeue();
        if (export == null)
            return;
        await export.Export();
        ExportQueue.RemoveAt(0);
        if (ExportQueue.Count > 0)
            StartExport();
    }
}