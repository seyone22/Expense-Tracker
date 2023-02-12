using System.Drawing;
using Expense_Tracker_v1._0.Services;
using Expense_Tracker_v1._0.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Color = Windows.UI.Color;

namespace Expense_Tracker_v1._0.Views;

public sealed partial class DashboardPage : Page
{
    //public Color c;
    //public SolidColorBrush Fill = new SolidColorBrush(Color.FromArgb());

    public List<Int32> AccountList = new()
    {
        1,
        2,
        3,
        4,
        5,
    };

    public DashboardViewModel ViewModel
    {
        get;
    }

    public DashboardPage()
    {
        ViewModel = App.GetService<DashboardViewModel>();
        InitializeComponent();

        //Random rand = new();
        //int i = rand.Next(CardColors.Count);
        //KnownColor.
        //c = CardColors[i];
    }
    
    //public static List<Color> CardColors = new()
    //{
    //    Color.FromKnownColor(KnownColor.Azure),
    //    Color.FromKnownColor(KnownColor.Gray),
    //};


}
