using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Contracts.Services.Realtime;
using Microsoft.AspNetCore.SignalR.Client;

namespace BistroQ.Service.Realtime;

public class CheckoutRealTimeService : ICheckoutRealTimeService
{
    private readonly HubConnection _hubConnection;

    private readonly IAuthService _authService;

    public event Action<int> OnPaymentInitiated;

    public event Action OnPaymentCompleted;

    public event Action<int, int> OnNewPayment;

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
        _hubConnection.On<int>("PaymentInitiated", (paymentUrl) =>
            OnPaymentInitiated?.Invoke(paymentUrl));

        // Notifies the client that the payment has been completed
        _hubConnection.On("PaymentCompleted", () =>
            OnPaymentCompleted?.Invoke());

        // Notifies the cashier that a new payment has been requested
        _hubConnection.On<int, int>("NewPayment", (tableId, zoneId) =>
            OnNewPayment?.Invoke(tableId, zoneId));
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
            OnPaymentInitiated = null;
            OnPaymentCompleted = null;
            OnNewPayment = null;
        }
    }

    // Call by client
    public async Task NotifyPaymentRequestedAsync(int tableId, int zoneId)
    {
        await _hubConnection.InvokeAsync("InitialPayment", tableId, zoneId);
    }

    // Call by cashier
    public async Task NotifyPaymentCompletedAsync(int tableId, int zoneId)
    {
        await _hubConnection.InvokeAsync("CompletePayment", tableId, zoneId);
    }
}
