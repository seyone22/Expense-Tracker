using Expense_Tracker_v1._0.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Expense_Tracker_v1._0.Views;

public sealed partial class AccountsListPage : Page
{
    public AccountsListViewModel ViewModel
    {
        get;
    }

    public AccountsListPage()
    {
        ViewModel = App.GetService<AccountsListViewModel>();
        InitializeComponent();
    }
}
