using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.Enums;
using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Commons;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Dispatching;
using System.Diagnostics;

namespace BistroQ.Presentation.ViewModels.CashierTable;

public partial class TableOrderDetailViewModel :
    ObservableObject,
    IRecipient<TableSelectedMessage>,
    IRecipient<TableStateChangedMessage>,
    IDisposable
{
    private readonly DispatcherQueue dispatcherQueue;
    private readonly IOrderDataService _orderDataService;
    private readonly IMapper _mapper;
    private readonly IMessenger _messenger;

    public TableOrderDetailViewModel(IOrderDataService orderDataService, IMapper mapper, IMessenger messenger)
    {
        _orderDataService = orderDataService;
        _mapper = mapper;
        _messenger = messenger;
        dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        Timer = new TimeCounterViewModel();
        _messenger.RegisterAll(this);
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCheckoutEnabled))]
    [NotifyPropertyChangedFor(nameof(DoesNotHaveOrder))]
    [NotifyPropertyChangedFor(nameof(DoesNotHaveOrderDetail))]
    [NotifyPropertyChangedFor(nameof(HasOrderDetail))]
    private OrderViewModel _order;

    public TimeCounterViewModel Timer { get; }

    public bool DoesNotHaveOrder => Order == null;

    public bool DoesNotHaveOrderDetail => Order != null && Order.OrderItems.Count <= 0;

    public bool HasOrderDetail => Order != null && Order.OrderItems.Count > 0;

    public bool IsCheckoutEnabled => Order != null && Order.Status == OrderStatus.Pending;

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

    [RelayCommand]
    public void CheckoutRequestedCommand()
    {
        _messenger.Send(new CompleteCheckoutRequestedMessage(Order.TableId));
    }

    public void Receive(TableSelectedMessage message)
    {
        OnTableChangedAsync(message.TableId);
    }

    public void Dispose()
    {
        _messenger.UnregisterAll(this);
    }

    public void Receive(TableStateChangedMessage message)
    {
        dispatcherQueue.TryEnqueue(async () =>
        {
            if (message.TableId == Order.TableId)
            {
                switch (message.State)
                {
                    case CashierTableState.Available:
                        Order = null;
                        Timer.Reset();
                        break;
                    case CashierTableState.Occupied:
                        Timer.Start();
                        break;
                    case CashierTableState.CheckoutPending:
                        await OnTableChangedAsync(message.TableId);
                        var updatedOrder = Order.Clone();
                        updatedOrder.Status = OrderStatus.Pending;
                        Order = updatedOrder;
                        Timer.Stop();
                        break;
                }
            }
        });
    }
}