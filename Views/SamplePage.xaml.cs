using Expense_Tracker_v1._0.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Expense_Tracker_v1._0.Views;

public sealed partial class SamplePage : Page
{
    public SampleViewModel ViewModel
    {
        get;
    }

    public SamplePage()
    {
        ViewModel = App.GetService<SampleViewModel>();
        InitializeComponent();
    }
}
