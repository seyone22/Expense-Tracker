using Expense_Tracker_v1._0.Contracts.Services;
using Expense_Tracker_v1._0.Helpers;
using Expense_Tracker_v1._0.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.System;
using Windows.Globalization.NumberFormatting;
using Expense_Tracker_v1._0.Core.Models;
using Expense_Tracker_v1._0.Services;
using Expense_Tracker_v1._0.Core.Services;
using Windows.Storage.Pickers;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Expense_Tracker_v1._0.Views;

public sealed partial class ShellPage : Page
{
    public ObservableCollection<Pool> Source { get; } = new ObservableCollection<Pool>();
    public ShellViewModel ViewModel
    {
        get;
    }

    public ShellPage(ShellViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        ViewModel.NavigationService.Frame = NavigationFrame;

        // TODO: Set the title bar icon by updating /Assets/WindowIcon.ico.
        // A custom title bar is required for full window theme and Mica support.
        // https://docs.microsoft.com/windows/apps/develop/title-bar?tabs=winui3#full-customization
        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_Activated;
        AppTitleBarText.Text = "AppDisplayName".GetLocalized();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme);

        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));

        ShellMenuBarSettingsButton.AddHandler(UIElement.PointerPressedEvent, new PointerEventHandler(ShellMenuBarSettingsButton_PointerPressed), true);
        ShellMenuBarSettingsButton.AddHandler(UIElement.PointerReleasedEvent, new PointerEventHandler(ShellMenuBarSettingsButton_PointerReleased), true);

        //Check if a database is loaded
        ShowNewPoolDialog();

        //Load up accounts for new account dialog
        //Accounts.Clear();

        //var data = (IEnumerable<Account>)_accountDataService.GetGridDataAsync();

        //foreach (var item in data)
        //{
        //    Accounts.Add(item);
        //}
    }

    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        var resource = args.WindowActivationState == WindowActivationState.Deactivated ? "WindowCaptionForegroundDisabled" : "WindowCaptionForeground";

        AppTitleBarText.Foreground = (SolidColorBrush)App.Current.Resources[resource];
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        ShellMenuBarSettingsButton.RemoveHandler(UIElement.PointerPressedEvent, (PointerEventHandler)ShellMenuBarSettingsButton_PointerPressed);
        ShellMenuBarSettingsButton.RemoveHandler(UIElement.PointerReleasedEvent, (PointerEventHandler)ShellMenuBarSettingsButton_PointerReleased);
    }

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }

    private void ShellMenuBarSettingsButton_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        AnimatedIcon.SetState((UIElement)sender, "PointerOver");
    }

    private void ShellMenuBarSettingsButton_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        AnimatedIcon.SetState((UIElement)sender, "Pressed");
    }

    private void ShellMenuBarSettingsButton_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        AnimatedIcon.SetState((UIElement)sender, "Normal");
    }

    private void ShellMenuBarSettingsButton_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        AnimatedIcon.SetState((UIElement)sender, "Normal");
    }

    public async void AddTransaction_Click(object sender, RoutedEventArgs e)
    {
        var result = await addTransactionDialog.ShowAsync(); //no error, just shows squiggly for some reason.
    }

    public async void AddAccount_Click(object sender, RoutedEventArgs e)
    {
        var result = await addAccountDialog.ShowAsync(); //no error, just shows squiggly for some reason.
    }

    public async void ShowNewPoolDialog()
    {
        ExistingPoolsDataService e = new();
        //var data = await e.Populate();

        //foreach (var item in data)
        //{
        //    Source.Add(item);
        //}

        var result = await startupDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            var IsNewFile = true;
            var f = new FileService();
            var s = new SqliteDataService();
            var d = new DashboardViewModel();

            if (f.Exists(newPoolName.Text))
            {
                IsNewFile = false;
            }

            await s.InitializeDatabaseAsync(f.CleanFileName(newPoolName.Text), IsNewFile);
            await d.LoadDashboardAsync();
            await ExistingPoolsDataService.setCurrentPool();
        }

        if (result == ContentDialogResult.Secondary)
        {
            var filePicker = new FileOpenPicker();

            // Get the current window's HWND by passing in the Window object
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);

            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);

            // Use file picker like normal!
            filePicker.FileTypeFilter.Add("*");
            var file = await filePicker.PickSingleFileAsync();
        }
    }//t

    public async void ShowNewPoolDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
    {
        var f = new FileService();
        ContentDialog poolExistsDialog = new ContentDialog
        {
            Title = "The pool you have selected already exists.",
            Content = "Do you want to open the existing file?",
            CloseButtonText = "Cancel",
            PrimaryButtonText = "Open"
        };

        ContentDialogResult openExisting = ContentDialogResult.None;
        if (f.Exists(newPoolName.Text))
        {
            openExisting = await poolExistsDialog.ShowAsync();
        }

        if (openExisting == ContentDialogResult.Primary)
        {

        }
    }

    private void SetNumberBoxNumberFormatter()
    {
        IncrementNumberRounder rounder = new IncrementNumberRounder();
        rounder.Increment = 0.25;
        rounder.RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp;
        
        DecimalFormatter formatter = new DecimalFormatter();
        formatter.IntegerDigits = 1;
        formatter.FractionDigits = 2;
        formatter.NumberRounder = rounder;
        valueInput.NumberFormatter = formatter;
    }

    private void addTransactionDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {

        Transaction newTx = new Transaction(fromAccountInput.Text, payeeInput.Text, dateInput.Date.Value.DateTime, Convert.ToDouble(valueInput.Text));
        SqliteDataService.PushTransaction(newTx);
        SqliteDataService.UpdateAccount(fromAccountInput.Text, Convert.ToDouble(valueInput.Text));
        //We have to refresh our datasource too.
        //Calculates dues for the account
        //CalculateDues()
    }

    private void addAccountDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        Account newAc = new Account(accountNameInput.Text, Convert.ToDouble(balanceInput.Text));
        SqliteDataService.PushAccount(newAc);
        SqliteDataService.UpdatePoolAddMember();
        //We have to refresh our datasource too.
    }
}