using CommunityToolkit.Mvvm.ComponentModel;
using Expense_Tracker_v1._0.Contracts.ViewModels;
using Expense_Tracker_v1._0.Core.Models;
using Expense_Tracker_v1._0.Services;
using Expense_Tracker_v1._0.Views;
using Windows.UI.Notifications;

namespace Expense_Tracker_v1._0.ViewModels;

public class DashboardViewModel : ObservableRecipient, INavigationAware
{
    public double PoolPerPerson;

    public Pool current = new();
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
        current = SqliteDataService.GetCurrentPool();

        current.balance = SqliteDataService.CalculatePoolTotal();
        PoolPerPerson = current.balance / current.personCount;

        await Task.CompletedTask;
    }
}
