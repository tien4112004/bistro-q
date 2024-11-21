using BistroQ.Contracts.ViewModels;
using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Models.Entities;
using BistroQ.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace BistroQ.ViewModels.CashierTable;

public partial class CashierTableViewModel : ObservableObject, INavigationAware
{
    private readonly IOrderDataService _orderDataService;

    private readonly IZoneDataService _zoneDataService;

    public ICommand SelectTableCommand { get; }

    public ICommand SelectZoneCommand { get; }

    public ZoneOverviewViewModel ZoneOverviewVM { get; }

    public ZoneTableGridViewModel ZoneTableGridVM { get; }

    public TableOrderDetailViewModel TableOrderDetailVM { get; }

    public CashierTableViewModel(
        IOrderDataService orderDataService, 
        IZoneDataService zoneDataService, 
        ZoneOverviewViewModel zoneOverview, 
        ZoneTableGridViewModel zoneTableGrid,
        TableOrderDetailViewModel tableOrderDetailVM
        )
    {
        _orderDataService = orderDataService;
        _zoneDataService = zoneDataService;
        SelectTableCommand = new RelayCommand<int>(OnTableSelected);
        SelectZoneCommand = new RelayCommand<ZoneStateEventArgs>(OnZoneSelected);
        ZoneOverviewVM = zoneOverview;
        ZoneTableGridVM = zoneTableGrid;
        TableOrderDetailVM = tableOrderDetailVM;    
    }

    public async void OnZoneSelected(ZoneStateEventArgs e)
    {
        await ZoneTableGridVM.OnZoneChangedAsync(e.ZoneId, e.Type);
        if (ZoneTableGridVM.SelectedTable != null)
        {
            await TableOrderDetailVM.OnTableChangedAsync(ZoneTableGridVM.SelectedTable.TableId);
        }
    }

    public async void OnTableSelected(int tableId)
    {
        await TableOrderDetailVM.OnTableChangedAsync(tableId);
    }

    public async void OnNavigatedTo(object parameter)
    {
        await ZoneOverviewVM.InitializeAsync();
        await ZoneTableGridVM.OnZoneChangedAsync(ZoneOverviewVM.SelectedZone.ZoneId, "All");
        if (ZoneTableGridVM.SelectedTable != null)
        {
            await TableOrderDetailVM.OnTableChangedAsync(ZoneTableGridVM.SelectedTable.TableId);
        }
    }

    public void OnNavigatedFrom()
    {
        //
    }
}
