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

    public CashierTableViewModel(
        IOrderDataService orderDataService, 
        IZoneDataService zoneDataService, 
        ZoneOverviewViewModel zoneOverview, 
        ZoneTableGridViewModel zoneTableGrid
        )
    {
        _orderDataService = orderDataService;
        _zoneDataService = zoneDataService;
        SelectTableCommand = new RelayCommand<int>(SelectTable);
        SelectZoneCommand = new RelayCommand<ZoneStateEventArgs>(OnZoneSelected);
        ZoneOverviewVM = zoneOverview;
        ZoneTableGridVM = zoneTableGrid;
    }

    public async void OnZoneSelected(ZoneStateEventArgs e)
    {
        await ZoneTableGridVM.OnZoneChangedAsync(e.ZoneId, e.Type);
    }


    public async void SelectTable(int tableId)
    {
        Debug.WriteLine("SELECT TABLE" + tableId.ToString());   
    }

    public async void OnNavigatedTo(object parameter)
    {
        await ZoneOverviewVM.InitializeAsync();
        await ZoneTableGridVM.OnZoneChangedAsync(ZoneOverviewVM.SelectedZone.ZoneId, "All");
    }

    public void OnNavigatedFrom()
    {
        //
    }
}
