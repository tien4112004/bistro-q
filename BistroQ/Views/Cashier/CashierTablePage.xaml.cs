using BistroQ.ViewModels.Cashier;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Views.Cashier;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class CashierTablePage : Page
{
    public CashierTableViewModel ViewModel { get; }
    public CashierTablePage()
    {
        ViewModel = App.GetService<CashierTableViewModel>();
        this.InitializeComponent();
    }

    private void SelectTable_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            var tableId = int.Parse(button.Tag.ToString());
            ViewModel.SelectTableCommand.Execute(tableId);
        }
    }
}
