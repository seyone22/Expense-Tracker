using Microsoft.UI.Xaml.Controls;

namespace Expense_Tracker_v1._0.Helpers;

public static class FrameExtensions
{
    public static object? GetPageViewModel(this Frame frame) => frame?.Content?.GetType().GetProperty("ViewModel")?.GetValue(frame.Content, null);
}
