using Expense_Tracker_v1._0.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Windows.Devices.Input;
using static System.Net.Mime.MediaTypeNames;

namespace Expense_Tracker_v1._0.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class AccPage : Page
{
    public AccViewModel ViewModel
    {
        get;
    }

    public AccPage()
    {
        ViewModel = App.GetService<AccViewModel>();
        InitializeComponent();
    }



    
    private void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        AccViewModel viewModel = App.GetService<AccViewModel>();
    }

    
    private void ShowMenu(bool isTransient)
    {
        FlyoutShowOptions myOption = new FlyoutShowOptions();
        myOption.ShowMode = isTransient ? FlyoutShowMode.Transient : FlyoutShowMode.Standard;

    }

    private void MyImageButton_ContextRequested(Microsoft.UI.Xaml.UIElement sender, ContextRequestedEventArgs args)
    {
        // always show a context menu in standard mode
        ShowMenu(false);
    }

    private void MyImageButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ShowMenu((sender as Button).IsPointerOver);
    }
}
