using CommunityToolkit.Mvvm.ComponentModel;
using Expense_Tracker_v1._0.Contracts.ViewModels;
using Expense_Tracker_v1._0.Services;

namespace Expense_Tracker_v1._0.ViewModels;

public class DashboardViewModel : ObservableRecipient, INavigationAware
{
    public double PoolTotal, PoolPerPerson;
    public DashboardViewModel()
    {
    }

    public void OnNavigatedFrom() { }
    public void OnNavigatedTo(object parameter)
    {
        PoolTotal = SqliteDataService.CalculatePoolTotal();
        PoolPerPerson = PoolTotal / SqliteDataService.PoolCount();
    }
}
