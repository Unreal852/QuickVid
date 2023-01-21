using QuickVid.Services;
using QuickVid.ViewModels;

namespace QuickVid;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        RootNavigation.SetServiceProvider(ServiceProvider.Instance);

        DataContext = new MainViewModel();
    }
}