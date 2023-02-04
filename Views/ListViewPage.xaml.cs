using Expense_Tracker_v1._0.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Windows.Devices.Input;

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

    private void ListViewPage_MouseClick(object sender, MouseEventArgs e)
    {
        var r = TransactionsDG.HitTest(e.X, e.Y);
        CommandBarFlyout1.ShowAt(ContentArea);
    }
}
