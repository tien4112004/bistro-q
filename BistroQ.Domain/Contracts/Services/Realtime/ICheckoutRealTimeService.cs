namespace BistroQ.Domain.Contracts.Services.Realtime;

public interface ICheckoutRealTimeService
{
    event Action<int> OnCheckoutInitiated;
    event Action OnCheckoutCompleted;
    event Action<int, int> OnNewCheckout;

    Task StartAsync();
    Task StopAsync();
    Task NotifyCheckoutRequestedAsync(int tableId);
    Task NotifyCheckoutCompletedAsync(int tableId);
}
