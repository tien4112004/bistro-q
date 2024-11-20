using BistroQ.ViewModels.CashierTable;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Views.CashierTable;

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

    private void ZoneControl_ZoneSelectionChanged(object sender, int? e)
    {
        ViewModel.SelectZoneCommand.Execute(e);
    }

    private void ZoneControl_TypeSelectionChanged(object sender, string e)
    {
        ViewModel.SelectTypeCommand.Execute(e);
    }
}
