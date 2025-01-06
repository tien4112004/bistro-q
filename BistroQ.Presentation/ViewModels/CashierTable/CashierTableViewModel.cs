using BistroQ.Domain.Contracts.Services.Realtime;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Enums;
using BistroQ.Presentation.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace BistroQ.Presentation.ViewModels.CashierTable;

/// <summary>
/// ViewModel for managing the cashier's table view interface.
/// Coordinates between zone overview, table grid, and order details components.
/// Handles real-time checkout notifications and state management.
/// </summary>
/// <remarks>
/// Implements multiple interfaces for functionality:
/// - ObservableObject for MVVM pattern
/// - INavigationAware for page navigation
/// - IRecipient for handling checkout messages
/// - IDisposable for resource cleanup
/// </remarks>
public partial class CashierTableViewModel :
    ObservableObject,
    INavigationAware,
    IRecipient<CompleteCheckoutRequestedMessage>,
    IDisposable
{
    #region Public Properties
    /// <summary>
    /// View model for managing zone overview functionality.
    /// </summary>
    public ZoneOverviewViewModel ZoneOverviewVM;

    /// <summary>
    /// View model for managing the zone table grid display.
    /// </summary>
    public ZoneTableGridViewModel ZoneTableGridVM;

    /// <summary>
    /// View model for managing table order details.
    /// </summary>
    public TableOrderDetailViewModel TableOrderDetailVM;
    #endregion

    #region Private Fields
    /// <summary>
    /// Service for handling messaging between components.
    /// </summary>
    private readonly IMessenger _messenger;

    /// <summary>
    /// Service for managing real-time checkout operations.
    /// </summary>
    private readonly ICheckoutRealTimeService _checkoutService;
    #endregion

    #region Events
    /// <summary>
    /// Event raised when a new checkout is requested.
    /// </summary>
    /// <remarks>
    /// Provides the table number and zone name where the checkout was requested.
    /// </remarks>
    public event EventHandler<(int tableNumber, string zoneName)> NewCheckoutNotification;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the CashierTableViewModel class.
    /// </summary>
    /// <param name="zoneOverview">View model for zone overview.</param>
    /// <param name="zoneTableGrid">View model for zone table grid.</param>
    /// <param name="tableOrderDetailVM">View model for table order details.</param>
    /// <param name="messenger">Service for messaging between components.</param>
    /// <param name="checkoutService">Service for real-time checkout operations.</param>
    public CashierTableViewModel(
        ZoneOverviewViewModel zoneOverview,
        ZoneTableGridViewModel zoneTableGrid,
        TableOrderDetailViewModel tableOrderDetailVM,
        IMessenger messenger,
        ICheckoutRealTimeService checkoutService
        )
    {
        ZoneOverviewVM = zoneOverview;
        ZoneTableGridVM = zoneTableGrid;
        TableOrderDetailVM = tableOrderDetailVM;
        _messenger = messenger;
        _checkoutService = checkoutService;
        _checkoutService.OnNewCheckout += (tableId, tableNumber, zoneName) =>
        {
            _messenger.Send(new ZoneStateChangedMessage(zoneName, true));
            _messenger.Send(new TableStateChangedMessage(tableId, CashierTableState.CheckoutPending));
            NewCheckoutNotification?.Invoke(this, (tableNumber, zoneName));
        };

        _messenger.RegisterAll(this);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Handles navigation to this page by initializing components and starting checkout service.
    /// </summary>
    /// <param name="parameter">Navigation parameter.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task OnNavigatedTo(object parameter)
    {
        await ZoneOverviewVM.InitializeAsync();
        await _checkoutService.StartAsync();
    }

    /// <summary>
    /// Handles navigation from this page by cleaning up resources and stopping checkout service.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task OnNavigatedFrom()
    {
        Dispose();
        await _checkoutService.StopAsync();
    }

    /// <summary>
    /// Handles the completion of a checkout request for a table.
    /// </summary>
    /// <param name="message">Message containing the table ID for checkout completion.</param>
    public void Receive(CompleteCheckoutRequestedMessage message)
    {
        var tableId = message.TableId ?? 0;
        _checkoutService.NotifyCheckoutCompletedAsync(tableId);
        _messenger.Send(new TableStateChangedMessage(tableId, CashierTableState.Available));
    }

    /// <summary>
    /// Performs cleanup of resources used by the ViewModel.
    /// </summary>
    public void Dispose()
    {
        ZoneTableGridVM.Dispose();
        TableOrderDetailVM.Dispose();

        _checkoutService.OnNewCheckout -= (tableId, tableNumber, zoneName) =>
        {
            _messenger.Send(new ZoneStateChangedMessage(zoneName, true));
            _messenger.Send(new TableStateChangedMessage(tableId, CashierTableState.CheckoutPending));
            NewCheckoutNotification?.Invoke(this, (tableNumber, zoneName));
        };

        _messenger.UnregisterAll(this);
    }
    #endregion
}