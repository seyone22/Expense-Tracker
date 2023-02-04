using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Expense_Tracker_v1._0.Contracts.ViewModels;
using Expense_Tracker_v1._0.Core.Contracts.Services;
using Expense_Tracker_v1._0.Core.Models;

namespace Expense_Tracker_v1._0.ViewModels;

public class ListViewViewModel : ObservableRecipient, INavigationAware
{
    //private readonly ISampleDataService _sampleDataService;
    private readonly ITransactionDataService _transactionDataService;

    public ObservableCollection<Transaction> Source { get; } = new ObservableCollection<Transaction>();

    public ListViewViewModel(ITransactionDataService transactionDataService)
    {
        _transactionDataService = transactionDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        var data = await _transactionDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }


}
