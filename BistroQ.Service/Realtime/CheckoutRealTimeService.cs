using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Contracts.Services.Realtime;
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;

namespace BistroQ.Service.Realtime;

public class CheckoutRealTimeService : ICheckoutRealTimeService
{
    private readonly HubConnection _hubConnection;

    private readonly IAuthService _authService;

    public event Action<int> OnCheckoutInitiated;

    public event Action OnCheckoutCompleted;

    public event Action<int, int> OnNewCheckout;

    public bool IsConnected => _hubConnection.State == HubConnectionState.Connected;

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
        _hubConnection.On<int>("CheckoutInitiated", (paymentUrl) =>
            OnCheckoutInitiated?.Invoke(paymentUrl));

        // Notifies the client that the payment has been completed
        _hubConnection.On("CheckoutCompleted", () =>
            OnCheckoutCompleted?.Invoke());

        // Notifies the cashier that a new checkout has been requested
        _hubConnection.On<int, int>("NewCheckout", (tableId, zoneId) =>
            OnNewCheckout?.Invoke(tableId, zoneId));
    }

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

    // Call by client
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

    // Call by cashier
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
}
