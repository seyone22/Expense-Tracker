using Expense_Tracker_v1._0.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Windows.Devices.Input;
using static System.Net.Mime.MediaTypeNames;

namespace Expense_Tracker_v1._0.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class ListViewPage : Page
{
    public ListViewViewModel ViewModel
    {
        get;
    }

    public ListViewPage()
    {
        ViewModel = App.GetService<ListViewViewModel>();
        InitializeComponent();
    }



    
    private void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ListViewViewModel viewModel = App.GetService<ListViewViewModel>();
    }
}
