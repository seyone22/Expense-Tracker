using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Expense_Tracker_v1._0.Contracts.ViewModels;
using Expense_Tracker_v1._0.Core.Contracts.Services;
using Expense_Tracker_v1._0.Core.Models;

namespace Expense_Tracker_v1._0.ViewModels;

public class AccountsViewModel : ObservableRecipient
{
	private readonly IAccountDataService _accountDataService;
	public ObservableCollection<Account> Source { get; } = new ObservableCollection<Account>();

	public AccountsViewModel(IAccountDataService accountDataService)
	{
		_accountDataService = accountDataService;
	}
	public async void OnNavigatedTo(object parameter)
	{
		Source.Clear();
		// TODO: Replace with real data.
		var data = await _accountDataService.GetGridDataAsync();

		foreach (var item in data)
		{
			Source.Add(item);
		}
	}
}