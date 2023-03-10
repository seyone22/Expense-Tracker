using Expense_Tracker_v1._0.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Expense_Tracker_v1._0.Views;

public sealed partial class AddTransactionPage : Page
{
    public AddTransactionViewModel ViewModel
    {
        get;
    }

    public AddTransactionPage()
    {
        ViewModel = App.GetService<AddTransactionViewModel>();
        InitializeComponent();
    }
}
