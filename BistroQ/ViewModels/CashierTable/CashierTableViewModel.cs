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
    [ObservableProperty]
    private Order _currentOrder = null;
    
    [ObservableProperty]
    private ObservableCollection<Order> _orders;

    private readonly IOrderDataService _orderDataService;

    private readonly IZoneDataService _zoneDataService;

    public ICommand SelectTableCommand { get; }

    public ICommand SelectZoneCommand { get; }

    public ZoneOverviewViewModel ZoneOverviewViewModel { get; }

    public CashierTableViewModel(IOrderDataService orderDataService, IZoneDataService zoneDataService, ZoneOverviewViewModel zoneOverview)
    {
        _orderDataService = orderDataService;
        _zoneDataService = zoneDataService;
        SelectTableCommand = new RelayCommand<int>(SelectTable);
        SelectZoneCommand = new RelayCommand<ZoneStateEventArgs>(OnZoneSelected);
        ZoneOverviewViewModel = zoneOverview;
    }

    public async void OnZoneSelected(ZoneStateEventArgs e)
    {
        // Get tables by zoneId
        Debug.WriteLine("GET TABLE BY " + e.ZoneId.ToString());
        Debug.WriteLine("TYPE " + e.Type.ToString());
    }


    public async void SelectTable(int tableId)
    {
        CurrentOrder = Orders.FirstOrDefault(o => o.TableId == tableId);
        CurrentOrder.OrderDetails = (await _orderDataService.GetOrderByCashierAsync(tableId)).OrderDetails;
    }

    public async void OnNavigatedTo(object parameter)
    {
        await ZoneOverviewViewModel.InitializeAsync();
        Orders = new ObservableCollection<Order>(await _orderDataService.GetCurrentOrdersByCashierAsync());

        for (int i = 0; i < Orders.Count; i++)
        {
            var zone = await _zoneDataService.GetZoneByIdAsync(Orders[i].Table.ZoneId.Value);
            Orders[i].OrderDetails = (await _orderDataService.GetOrderByCashierAsync(Orders[i].TableId.Value)).OrderDetails;
            Orders[i].Table.ZoneName = zone.Name;
        }

        Orders = new ObservableCollection<Order>(Orders);
    }

    public void OnNavigatedFrom()
    {
        //
    }
}
