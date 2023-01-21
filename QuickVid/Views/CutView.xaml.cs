using System.Windows.Controls;
using QuickVid.ViewModels;

namespace QuickVid.Views;

public partial class CutView : UserControl
{
    public CutView()
    {
        InitializeComponent();
        DataContext = new CutViewModel();
    }
}