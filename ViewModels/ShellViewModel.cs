using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Expense_Tracker_v1._0.Contracts.Services;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;

namespace Expense_Tracker_v1._0.ViewModels;

public class ShellViewModel : ObservableRecipient
{
    private bool _isBackEnabled;

    public ICommand MenuFileExitCommand
    {
        get;
    }

    public ICommand MenuSettingsCommand
    {
        get;
    }

    public ICommand MenuViewsSampleCommand
    {
        get;
    }

    public ICommand MenuViewsPayablesCommand
    {
        get;
    }

    public ICommand MenuViewsListViewCommand
    {
        get;
    }

    public ICommand MenuViewsDashboardCommand
    {
        get;
    }

    public INavigationService NavigationService
    {
        get;
    }

    public bool IsBackEnabled
    {
        get => _isBackEnabled;
        set => SetProperty(ref _isBackEnabled, value);
    }

    public ShellViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;

        MenuFileExitCommand = new RelayCommand(OnMenuFileExit);
        MenuSettingsCommand = new RelayCommand(OnMenuSettings);
        MenuViewsSampleCommand = new RelayCommand(OnMenuViewsSample);
        MenuViewsPayablesCommand = new RelayCommand(OnMenuViewsPayables);
        MenuViewsListViewCommand = new RelayCommand(OnMenuViewsListView);
        MenuViewsDashboardCommand = new RelayCommand(OnMenuViewsDashboard);
    }

    private void OnNavigated(object sender, NavigationEventArgs e) => IsBackEnabled = NavigationService.CanGoBack;

    private void OnMenuFileExit() => Application.Current.Exit();

    private void OnMenuSettings() => NavigationService.NavigateTo(typeof(SettingsViewModel).FullName!);

    private void OnMenuViewsSample() => NavigationService.NavigateTo(typeof(SampleViewModel).FullName!);

    private void OnMenuViewsPayables() => NavigationService.NavigateTo(typeof(PayablesViewModel).FullName!);

    private void OnMenuViewsListView() => NavigationService.NavigateTo(typeof(ListViewViewModel).FullName!);

    private void OnMenuViewsDashboard() => NavigationService.NavigateTo(typeof(DashboardViewModel).FullName!);
}
