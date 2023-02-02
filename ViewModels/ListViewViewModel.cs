using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Expense_Tracker_v1._0.Contracts.ViewModels;
using Expense_Tracker_v1._0.Core.Contracts.Services;
using Expense_Tracker_v1._0.Core.Models;

namespace Expense_Tracker_v1._0.ViewModels;

public class ListViewViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

    public ListViewViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _sampleDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
