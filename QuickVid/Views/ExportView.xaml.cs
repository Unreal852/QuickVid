using System.Windows.Controls;
using QuickVid.ViewModels;

namespace QuickVid.Views;

/// <summary>
/// Logique d'interaction pour ExportView.xaml
/// </summary>
public partial class ExportView : UserControl
{
    public ExportView()
    {
        InitializeComponent();
        DataContext = new ExportViewModel();
    }

    public ExportViewModel ViewModel => (ExportViewModel)DataContext;
}
