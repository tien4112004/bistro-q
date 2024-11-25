using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Entities;
using BistroQ.Helpers;
using BistroQ.ViewModels.Commons;
using CommunityToolkit.Mvvm.ComponentModel;

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

    [ObservableProperty]
    private bool _isLoading = true;

    public async Task<Order?> OnTableChangedAsync(int? tableId)
    {
        IsLoading = true;
        var order = await TaskHelper.WithMinimumDelay(
            _orderDataService.GetOrderByCashierAsync(tableId ?? 0),
            200);

        IsLoading = false;
        Order = order ?? new Order();
        Timer.SetStartTime(Order?.StartTime ?? DateTime.Now);
        return order;
    }
}
