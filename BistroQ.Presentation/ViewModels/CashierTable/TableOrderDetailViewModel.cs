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

/// <summary>
/// ViewModel for managing and displaying table order details.
/// Handles order status updates, checkout operations, and timer management.
/// </summary>
/// <remarks>
/// Implements multiple interfaces for functionality:
/// - ObservableObject for MVVM pattern
/// - IRecipient for handling table selection and state messages
/// - IDisposable for resource cleanup
/// </remarks>
public partial class TableOrderDetailViewModel :
    ObservableObject,
    IRecipient<TableSelectedMessage>,
    IRecipient<TableStateChangedMessage>,
    IDisposable
{
    #region Private Fields
    /// <summary>
    /// Queue for dispatching UI updates.
    /// </summary>
    private readonly DispatcherQueue dispatcherQueue;

    /// <summary>
    /// Service for managing order data operations.
    /// </summary>
    private readonly IOrderDataService _orderDataService;

    /// <summary>
    /// Service for object mapping operations.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Service for handling messaging between components.
    /// </summary>
    private readonly IMessenger _messenger;

    /// <summary>
    /// Current order being displayed.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCheckoutEnabled))]
    [NotifyPropertyChangedFor(nameof(DoesNotHaveOrder))]
    [NotifyPropertyChangedFor(nameof(DoesNotHaveOrderDetail))]
    [NotifyPropertyChangedFor(nameof(HasOrderDetail))]
    private OrderViewModel _order;

    /// <summary>
    /// Flag indicating whether data is being loaded.
    /// </summary>
    [ObservableProperty]
    private bool _isLoading = true;
    #endregion

    #region Public Properties
    /// <summary>
    /// Timer for tracking order duration.
    /// </summary>
    public TimeCounterViewModel Timer { get; }

    /// <summary>
    /// Gets whether there is no current order.
    /// </summary>
    public bool DoesNotHaveOrder => Order == null;

    /// <summary>
    /// Gets whether there is an order but no order details.
    /// </summary>
    public bool DoesNotHaveOrderDetail => Order != null && Order.OrderItems.Count <= 0;

    /// <summary>
    /// Gets whether there is an order with details.
    /// </summary>
    public bool HasOrderDetail => Order != null && Order.OrderItems.Count > 0;

    /// <summary>
    /// Gets whether checkout is enabled for the current order.
    /// </summary>
    public bool IsCheckoutEnabled => Order != null && Order.Status == OrderStatus.Pending;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the TableOrderDetailViewModel class.
    /// </summary>
    /// <param name="orderDataService">Service for order data operations.</param>
    /// <param name="mapper">Service for object mapping.</param>
    /// <param name="messenger">Service for messaging between components.</param>
    public TableOrderDetailViewModel(IOrderDataService orderDataService, IMapper mapper, IMessenger messenger)
    {
        _orderDataService = orderDataService;
        _mapper = mapper;
        _messenger = messenger;
        dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        Timer = new TimeCounterViewModel();
        _messenger.RegisterAll(this);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Updates the order details when a table changes.
    /// </summary>
    /// <param name="tableId">ID of the selected table.</param>
    /// <returns>The updated order view model, or null if not found.</returns>
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

    /// <summary>
    /// Command handler for checkout requests.
    /// </summary>
    [RelayCommand]
    public void CheckoutRequestedCommand()
    {
        _messenger.Send(new CompleteCheckoutRequestedMessage(Order.TableId));
    }

    /// <summary>
    /// Handles table selection messages.
    /// </summary>
    /// <param name="message">Message containing the selected table ID.</param>
    public void Receive(TableSelectedMessage message)
    {
        OnTableChangedAsync(message.TableId);
    }

    /// <summary>
    /// Performs cleanup of resources.
    /// </summary>
    public void Dispose()
    {
        _messenger.UnregisterAll(this);
    }

    /// <summary>
    /// Handles table state change messages and updates the order accordingly.
    /// </summary>
    /// <param name="message">Message containing the table state change information.</param>
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
    #endregion
}