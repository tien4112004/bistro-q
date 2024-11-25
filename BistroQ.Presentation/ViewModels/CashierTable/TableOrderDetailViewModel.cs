using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.ViewModels.Commons;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;

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

    public async Task<OrderViewModel?> OnTableChangedAsync(int? tableId)
    {
        IsLoading = true;
        try
        {
            var order = await TaskHelper.WithMinimumDelay(
                _orderDataService.GetOrderByCashierAsync(tableId ?? 0),
                200);

            Order = _mapper.Map<OrderViewModel>(order);
            Debug.WriteLine(Order.OrderItems[0].Total);
            Timer.SetStartTime(Order.StartTime ?? DateTime.Now);
            return Order;
        }
        catch (Exception ex)
        {
            Order = new OrderViewModel();
            Timer.SetStartTime(DateTime.Now);

            Debug.WriteLine(ex.Message);
            return null;
        }
        finally
        {
            IsLoading = false;
        }

    }
}