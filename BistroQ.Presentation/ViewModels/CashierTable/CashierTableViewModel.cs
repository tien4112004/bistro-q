using BistroQ.Domain.Contracts.Services.Realtime;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;

namespace BistroQ.Presentation.ViewModels.CashierTable;

public partial class CashierTableViewModel : ObservableObject, INavigationAware, IRecipient<CheckoutRequestedMessage>, IDisposable
{
    public ZoneOverviewViewModel ZoneOverviewVM;
    public ZoneTableGridViewModel ZoneTableGridVM;
    public TableOrderDetailViewModel TableOrderDetailVM;
    private readonly IMessenger _messenger;
    private readonly ICheckoutRealTimeService _checkoutService;

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
        _checkoutService.OnNewPayment += (tableId, zoneId) =>
        {
            Debug.WriteLine($"New payment for table {tableId} in zone {zoneId}");
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

    public void Receive(CheckoutRequestedMessage message)
    {
        _checkoutService.NotifyPaymentCompletedAsync(message.TableId ?? 0, ZoneOverviewVM.SelectedZone.ZoneId ?? 0);
    }

    public void Dispose()
    {
        ZoneTableGridVM.Dispose();
        TableOrderDetailVM.Dispose();
        _messenger.UnregisterAll(this);
    }
}
