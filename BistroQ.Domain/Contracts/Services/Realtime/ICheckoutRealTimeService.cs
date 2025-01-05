namespace BistroQ.Domain.Contracts.Services.Realtime;

/// <summary>
/// Interface defining real-time communication service for checkout operations.
/// Manages bidirectional communication between clients and server for payment processing.
/// </summary>
public interface ICheckoutRealTimeService
{
    /// <summary>
    /// Event triggered when a checkout process is initiated, providing the payment URL.
    /// </summary>
    /// <param name="paymentUrl">URL for the payment gateway or payment data</param>
    event Action<string> OnCheckoutInitiated;

    /// <summary>
    /// Event triggered when a checkout process is successfully completed.
    /// </summary>
    event Action OnCheckoutCompleted;

    /// <summary>
    /// Event triggered when a new checkout is requested by a client.
    /// </summary>
    /// <param name="tableId">ID of the table requesting checkout</param>
    /// <param name="tableNumber">Display number of the table</param>
    /// <param name="zoneName">Name of the zone where the table is located</param>
    event Action<int, int, string> OnNewCheckout;

    /// <summary>
    /// Initiates the SignalR connection to the server.
    /// </summary>
    /// <returns>A task representing the asynchronous operation</returns>
    Task StartAsync();

    /// <summary>
    /// Terminates the SignalR connection and cleans up event handlers.
    /// </summary>
    /// <returns>A task representing the asynchronous operation</returns>
    Task StopAsync();

    /// <summary>
    /// Notifies the server that a client has requested to begin the checkout process.
    /// </summary>
    /// <param name="tableId">ID of the table requesting checkout</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task NotifyCheckoutRequestedAsync(int tableId);

    /// <summary>
    /// Notifies the server that a cashier has completed the checkout process.
    /// </summary>
    /// <param name="tableId">ID of the table completing checkout</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task NotifyCheckoutCompletedAsync(int tableId);
}