using BistroQ.Contracts.ViewModels;
using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos.Tables;
using BistroQ.Core.Dtos.Zones;
using BistroQ.Core.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BistroQ.ViewModels.Cashier;

public partial class CashierTableViewModel : ObservableObject, INavigationAware
{
    [ObservableProperty]
    private Order _currentOrder = null;

    [ObservableProperty]
    private ZoneDetailDto _currentZone;

    [ObservableProperty]
    private ObservableCollection<Order> _orders;

    [ObservableProperty]
    private ObservableCollection<ZoneDetailDto> _zones;

    private readonly IOrderDataService _orderDataService;

    private readonly IZoneDataService _zoneDataService;

    public ICommand SelectTableCommand { get; }

    public ICommand SelectZoneCommand { get; }

    public CashierTableViewModel(IOrderDataService orderDataService, IZoneDataService zoneDataService)
    {
        _orderDataService = orderDataService;
        _zoneDataService = zoneDataService;
        SelectTableCommand = new RelayCommand<int>(SelectTable);
        SelectZoneCommand = new RelayCommand<int>(SelectZone);
    }

    public async void SelectTable(int tableId)
    {
        CurrentOrder = await _orderDataService.GetOrderByCashierAsync(tableId);
    }

    public async void SelectZone(int zoneId)
    {
        CurrentZone = await _zoneDataService.GetZoneByIdAsync(zoneId);
        Orders = new ObservableCollection<Order>(await _orderDataService.GetCurrentOrdersByCashierAsync(zoneId));
        
        if (Orders.Count > 0)
        {
            SelectTable(Orders.First().TableId ?? 0);
        }
    }

    public async void OnNavigatedTo(object parameter)
    {
        var zones = (await _zoneDataService.GetZonesAsync(new ZoneCollectionQueryParams { Page = 1, Size = 100 })).ToList();

        Zones = new ObservableCollection<ZoneDetailDto>(zones);

        if (Zones.Count == 0)
        {
            return;
        }

        var zoneId = zones.FirstOrDefault()?.ZoneId ?? 0;

        CurrentZone = await _zoneDataService.GetZoneByIdAsync(zoneId);
        Orders = new ObservableCollection<Order>(await _orderDataService.GetCurrentOrdersByCashierAsync(zoneId));

        if (Orders.Count == 0)
        {
            return;
        }
        var tableId = Orders.First().TableId ?? 0;

        CurrentOrder = Orders.FirstOrDefault(o => o.TableId == tableId) ?? new Order();
        CurrentOrder.OrderDetails = (await _orderDataService.GetOrderByCashierAsync(tableId)).OrderDetails;

    }

    public void OnNavigatedFrom()
    {
        CurrentOrder = new Order();
        Orders = new ObservableCollection<Order>();
        Zones = new ObservableCollection<ZoneDetailDto>();
        CurrentZone = new ZoneDetailDto();
    }
}
