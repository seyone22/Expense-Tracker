using CommunityToolkit.Mvvm.ComponentModel;
using Expense_Tracker_v1._0.Contracts.ViewModels;
using Expense_Tracker_v1._0.Services;
using Expense_Tracker_v1._0.Views;
using Windows.UI.Notifications;

namespace Expense_Tracker_v1._0.ViewModels;

public class DashboardViewModel : ObservableRecipient, INavigationAware
{
    public double PoolTotal, PoolPerPerson;
    public int PoolCount;
    public string PoolName;
    public DashboardViewModel()
    {
    }

    public void OnNavigatedFrom()
    {
    }
    public void OnNavigatedTo(object parameter)
    {
        if (!string.IsNullOrEmpty(SqliteDataService.GetCurrentDatabaseName()))
        {
            LoadDashboardAsync();
        }
    }

    public async Task LoadDashboardAsync()
    {

        PoolTotal = SqliteDataService.CalculatePoolTotal();
        PoolPerPerson = PoolTotal / SqliteDataService.PoolCount();
        PoolName = SqliteDataService.GetCurrentDatabaseName();
        PoolCount = 2;
        await Task.CompletedTask;
    }
}
