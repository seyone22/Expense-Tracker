using Expense_Tracker_v1._0.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Expense_Tracker_v1._0.Views;

public sealed partial class AccountsPage : Page
{
    public AccountsViewModel ViewModel
    {
        get;
    }

    public AccountsPage()
    {
        ViewModel = App.GetService<AccountsViewModel>();
        InitializeComponent();
    }
}