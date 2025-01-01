using BistroQ.Domain.Contracts.Services.Realtime;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Enums;
using BistroQ.Presentation.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace BistroQ.Presentation.ViewModels.CashierTable;

public partial class CashierTableViewModel :
    ObservableObject,
    INavigationAware,
    IRecipient<CompleteCheckoutRequestedMessage>,
    IDisposable
{
    public ZoneOverviewViewModel ZoneOverviewVM;
    public ZoneTableGridViewModel ZoneTableGridVM;
    public TableOrderDetailViewModel TableOrderDetailVM;
    private readonly IMessenger _messenger;
    private readonly ICheckoutRealTimeService _checkoutService;

    public event EventHandler<(int tableNumber, string zoneName)> NewCheckoutNotification;

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
            _messenger.Send(new TableStateChangedMessage(tableId, CashierTableState.CheckoutPending));
            NewCheckoutNotification?.Invoke(this, (tableNumber, zoneName));
        };

        _messenger.RegisterAll(this);
    }

    public async Task OnNavigatedTo(object parameter)
    {
        await ZoneOverviewVM.InitializeAsync();
        await _checkoutService.StartAsync();
    }

    public async Task OnNavigatedFrom()
    {
        Dispose();
        await _checkoutService.StopAsync();
    }

    public void Receive(CompleteCheckoutRequestedMessage message)
    {
        _checkoutService.NotifyCheckoutCompletedAsync(message.TableId ?? 0);
    }

    public void Dispose()
    {
        ZoneTableGridVM.Dispose();
        TableOrderDetailVM.Dispose();

        _checkoutService.OnNewCheckout -= (tableId, tableNumber, zoneName) =>
        {
            _messenger.Send(new TableStateChangedMessage(tableId, CashierTableState.CheckoutPending));
            NewCheckoutNotification?.Invoke(this, (tableNumber, zoneName));
        };

        _messenger.UnregisterAll(this);
    }
}
