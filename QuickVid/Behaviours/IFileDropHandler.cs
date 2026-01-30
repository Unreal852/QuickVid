using System.Threading.Tasks;

namespace QuickVid.Behaviours;

internal interface IFileDropHandler
{
    Task OnFilesDropped(string[] files);
}