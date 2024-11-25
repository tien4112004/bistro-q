using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Models.Entities;
using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.ViewModels.Commons;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.CashierTable;

public partial class TableOrderDetailViewModel : ObservableObject
{
    private readonly IOrderDataService _orderDataService;
    private readonly IMapper _mapper;

    public TableOrderDetailViewModel(IOrderDataService orderDataService, IMapper mapper)
    {
        _orderDataService = orderDataService;
        _mapper = mapper;
        Timer = new TimeCounterViewModel();
    }

    [ObservableProperty]
    private OrderViewModel _order;

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
        Order = _mapper.Map<OrderViewModel>(order);
        Timer.SetStartTime(Order?.StartTime ?? DateTime.Now);
        return order;
    }
}