
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Contracts.Services.Realtime;
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;

namespace BistroQ.Service.Realtime;

/// <summary>
/// Implementation of real-time checkout service using SignalR.
/// Handles bidirectional communication between clients and server for payment processing.
/// </summary>
public class CheckoutRealTimeService : ICheckoutRealTimeService
{
    #region Private Fields
    /// <summary>
    /// SignalR hub connection instance for real-time communication.
    /// </summary>
    private readonly HubConnection _hubConnection;

    /// <summary>
    /// Service for handling authentication operations.
    /// </summary>
    private readonly IAuthService _authService;
    #endregion

    #region Events
    public event Action<string> OnCheckoutInitiated;
    public event Action OnCheckoutCompleted;
    public event Action<int, int, string> OnNewCheckout;
    #endregion

    #region Properties
    /// <summary>
    /// Gets whether the hub connection is currently established.
    /// </summary>
    public bool IsConnected => _hubConnection.State == HubConnectionState.Connected;
    #endregion

    #region Constructor
    public CheckoutRealTimeService(IAuthService authService)
    {
        _authService = authService;
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5256/hubs/checkout", options =>
            {
                options.AccessTokenProvider = async () => await authService.GetTokenAsync();
            })
            .WithAutomaticReconnect()
            .Build();

        // Shows the payment URL to the client
        _hubConnection.On<string>("CheckoutInitiated", (paymentData) =>
                OnCheckoutInitiated?.Invoke(paymentData));

        // Notifies the client that the payment has been completed
        _hubConnection.On("CheckoutCompleted", () =>
                OnCheckoutCompleted?.Invoke());

        // Notifies the cashier that a new checkout has been requested
        _hubConnection.On<int, int, string>("NewCheckout", (tableId, tableNumber, zoneName) =>
            OnNewCheckout?.Invoke(tableId, tableNumber, zoneName));
    }
    #endregion

    #region Public Methods
    public async Task StartAsync()
    {
        if (IsConnected) return;

        await _hubConnection.StartAsync();
    }

    public async Task StopAsync()
    {
        if (_hubConnection.State != HubConnectionState.Disconnected)
        {
            await _hubConnection.StopAsync();
            OnCheckoutInitiated = null;
            OnCheckoutCompleted = null;
            OnNewCheckout = null;
        }
    }

    public async Task NotifyCheckoutRequestedAsync(int tableId)
    {
        try
        {
            await _hubConnection.InvokeAsync("InitiateCheckout", tableId);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }

    public async Task NotifyCheckoutCompletedAsync(int tableId)
    {
        try
        {
            await _hubConnection.InvokeAsync("CompleteCheckout", tableId);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }
    #endregion
}