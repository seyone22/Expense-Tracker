﻿using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Expense_Tracker_v1._0.Contracts.Services;
using Expense_Tracker_v1._0.Core.Models;
using Expense_Tracker_v1._0.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Expense_Tracker_v1._0.ViewModels;

public class ShellViewModel : ObservableRecipient
{
    private readonly IAccountDataService _accountDataService;

    public ObservableCollection<Account> Accounts { get; } = new ObservableCollection<Account>();

    private bool _isBackEnabled;

    public ICommand MenuFileExitCommand
    {
        get;
    }

    public ICommand MenuViewsAddTransactionCommand
    {
        get;
    }
    public ICommand MenuViewsAddAccountCommand
    {
        get;
    }
    public ICommand MenuSettingsCommand
    {
        get;
    }

    public ICommand MenuViewsAccountsCommand
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

    public ICommand GoBackCommand
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

    public ShellViewModel(INavigationService navigationService, IAccountDataService accountDataService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        

        MenuFileExitCommand = new RelayCommand(OnMenuFileExit);
        MenuViewsAddTransactionCommand = new RelayCommand(OnMenuViewsAddTransaction);
        MenuSettingsCommand = new RelayCommand(OnMenuSettings);
        MenuViewsAccountsCommand = new RelayCommand(OnMenuViewsAccounts);
        MenuViewsPayablesCommand = new RelayCommand(OnMenuViewsPayables);
        MenuViewsListViewCommand = new RelayCommand(OnMenuViewsListView);
        MenuViewsDashboardCommand = new RelayCommand(OnMenuViewsDashboard);

        GoBackCommand = new RelayCommand(GoBack);

        _accountDataService = accountDataService;
    }
    public async void OnNavigatedTo(object parameter)
    {
        Accounts.Clear();

        var data = await _accountDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Accounts.Add(item);
        }
    }
    private void OnNavigated(object sender, NavigationEventArgs e) => IsBackEnabled = NavigationService.CanGoBack;

    private void OnMenuFileExit() => Application.Current.Exit();

    private void OnMenuViewsAddTransaction() => Application.Current.Exit();

    private void OnMenuSettings() => NavigationService.NavigateTo(typeof(SettingsViewModel).FullName!);

    private void OnMenuViewsAccounts() => NavigationService.NavigateTo(typeof(AccountsViewModel).FullName!);

    private void OnMenuViewsPayables() => NavigationService.NavigateTo(typeof(PayablesViewModel).FullName!);

    private void OnMenuViewsListView() => NavigationService.NavigateTo(typeof(ListViewViewModel).FullName!);

    private void OnMenuViewsDashboard() => NavigationService.NavigateTo(typeof(DashboardViewModel).FullName!);

    private void GoBack() => NavigationService.GoBack();

}
