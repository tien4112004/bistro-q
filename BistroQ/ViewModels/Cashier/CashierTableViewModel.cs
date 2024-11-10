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
    private ObservableCollection<Order> _orders;

    private readonly IOrderDataService _orderDataService;

    private readonly IZoneDataService _zoneDataService;

    public ICommand SelectTableCommand { get; }

    public CashierTableViewModel(IOrderDataService orderDataService, IZoneDataService zoneDataService)
    {
        _orderDataService = orderDataService;
        _zoneDataService = zoneDataService;
        SelectTableCommand = new RelayCommand<int>(SelectTable);
    }

    public async void SelectTable(int tableId)
    {
        CurrentOrder = Orders.FirstOrDefault(o => o.TableId == tableId);
        CurrentOrder.OrderDetails = (await _orderDataService.GetOrderByCashierAsync(tableId)).OrderDetails;
    }

    public async void OnNavigatedTo(object parameter)
    {
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
