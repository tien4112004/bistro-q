using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos.Tables;
using BistroQ.Core.Models.Entities;
using BistroQ.Core.Services;
using BistroQ.ViewModels.Commons;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.ViewModels.CashierTable;

public partial class TableOrderDetailViewModel : ObservableObject
{
    private readonly IOrderDataService _orderDataService;

    public TableOrderDetailViewModel(IOrderDataService orderDataService)
    {
        _orderDataService = orderDataService;
        Timer = new TimeCounterViewModel();
    }

    [ObservableProperty]
    private Order _order;

    public TimeCounterViewModel Timer { get; }

    public bool DoesNotHaveOrder => Order == null;

    public bool DoesNotHaveOrderDetail => Order != null && Order.OrderItems.Count <= 0;

    public bool HasOrderDetail => Order != null && Order.OrderItems.Count > 0;
 
    public async Task<Order?> OnTableChangedAsync(int? tableId)
    {
        if (tableId == null)
        {
            Order = new Order();
            Timer.Reset();
            Timer.SetStartTime(DateTime.Now);
            return null;
        }
        var order = await _orderDataService.GetOrderByCashierAsync((int)tableId);
        Order = order;
        Timer.SetStartTime(Order?.StartTime ?? DateTime.Now);
        return order;
    }
}
