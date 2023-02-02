using Expense_Tracker_v1._0.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Expense_Tracker_v1._0.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class PayablesPage : Page
{
    public PayablesViewModel ViewModel
    {
        get;
    }

    public PayablesPage()
    {
        ViewModel = App.GetService<PayablesViewModel>();
        InitializeComponent();
    }
}
