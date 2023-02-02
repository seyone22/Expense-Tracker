using Expense_Tracker_v1._0.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Expense_Tracker_v1._0.Views;

public sealed partial class DashboardPage : Page
{
    public DashboardViewModel ViewModel
    {
        get;
    }

    public DashboardPage()
    {
        ViewModel = App.GetService<DashboardViewModel>();
        InitializeComponent();
    }
}
