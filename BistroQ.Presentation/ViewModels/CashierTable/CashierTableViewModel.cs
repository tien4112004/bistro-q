using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;

namespace BistroQ.Presentation.ViewModels.CashierTable;

public partial class CashierTableViewModel : ObservableObject, INavigationAware, IRecipient<CheckoutRequestedMessage>
{
    public ZoneOverviewViewModel ZoneOverviewVM;
    public ZoneTableGridViewModel ZoneTableGridVM;
    public TableOrderDetailViewModel TableOrderDetailVM;
    private readonly IMessenger _messenger;

    public CashierTableViewModel(
        ZoneOverviewViewModel zoneOverview,
        ZoneTableGridViewModel zoneTableGrid,
        TableOrderDetailViewModel tableOrderDetailVM,
        IMessenger messenger
        )
    {
        ZoneOverviewVM = zoneOverview;
        ZoneTableGridVM = zoneTableGrid;
        TableOrderDetailVM = tableOrderDetailVM;
        _messenger = messenger;
        _messenger.RegisterAll(this);
    }

    public async void OnNavigatedTo(object parameter)
    {
        await ZoneOverviewVM.InitializeAsync();
    }

    public void OnNavigatedFrom()
    {
        //
    }

    public void Receive(CheckoutRequestedMessage message)
    {
        Debug.WriteLine("Checkout requested");
    }
}
