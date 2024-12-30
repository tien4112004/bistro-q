namespace BistroQ.Domain.Contracts.Services.Realtime;

public interface ICheckoutRealTimeService
{
    event Action<int> OnPaymentInitiated;
    event Action OnPaymentCompleted;
    event Action<int, int> OnNewPayment;

    Task StartAsync();
    Task StopAsync();
    Task NotifyPaymentRequestedAsync(int tableId, int zoneId);
    Task NotifyPaymentCompletedAsync(int tableId, int zoneId);
}
