using BistroQ.Presentation.Models;
using BistroQ.Presentation.ViewModels.CashierTable;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views.CashierTable;
public sealed partial class CashierTablePage : Page
{
    public CashierTableViewModel ViewModel { get; }
    public CashierTablePage()
    {
        ViewModel = App.GetService<CashierTableViewModel>();
        this.InitializeComponent();
    }

    private void ZoneControl_ZoneSelectionChanged(object sender, ZoneStateEventArgs e)
    {
        ViewModel.SelectZoneCommand.Execute(e);
    }

    private void GridControl_TableSelectionChanged(object sender, string? e)
    {
        ViewModel.SelectTableCommand.Execute(e);
    }

    private void TableOrderDetailsControl_CheckoutRequested(object sender, string? e)
    {
        ViewModel.CheckoutCommand.Execute(e);
    }
}