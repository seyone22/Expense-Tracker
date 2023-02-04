using CommunityToolkit.Mvvm.ComponentModel;

using Expense_Tracker_v1._0.Contracts.Services;
using Expense_Tracker_v1._0.ViewModels;
using Expense_Tracker_v1._0.Views;

using Microsoft.UI.Xaml.Controls;

namespace Expense_Tracker_v1._0.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new();

    public PageService()
    {
        Configure<DashboardViewModel, DashboardPage>();
        Configure<ListViewViewModel, ListViewPage>();
        Configure<PayablesViewModel, PayablesPage>();
        Configure<SampleViewModel, SamplePage>();
        Configure<SettingsViewModel, SettingsPage>();
        Configure<AddTransactionViewModel, AddTransactionPage>();
    }

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    private void Configure<VM, V>()
        where VM : ObservableObject
        where V : Page
    {
        lock (_pages)
        {
            var key = typeof(VM).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (_pages.Any(p => p.Value == type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}
