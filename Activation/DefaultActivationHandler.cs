using Expense_Tracker_v1._0.Contracts.Services;
using Expense_Tracker_v1._0.ViewModels;

using Microsoft.UI.Xaml;

namespace Expense_Tracker_v1._0.Activation;

public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the activation.
        return _navigationService.Frame?.Content == null;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        _navigationService.NavigateTo(typeof(DashboardViewModel).FullName!, args.Arguments);

        await Task.CompletedTask;
    }
}
