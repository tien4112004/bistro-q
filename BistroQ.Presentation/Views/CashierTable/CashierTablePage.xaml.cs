using BistroQ.Presentation.ViewModels.CashierTable;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views.CashierTable;
public sealed partial class CashierTablePage : Page
{
    public CashierTableViewModel ViewModel { get; }
    public CashierTablePage()
    {
        ViewModel = App.GetService<CashierTableViewModel>();
        this.InitializeComponent();
        ViewModel.NewCheckoutNotification += OnNewCheckoutNotification;
        this.Unloaded += (s, e) =>
        {
            ViewModel.NewCheckoutNotification -= OnNewCheckoutNotification;
            ViewModel.Dispose();
        };
    }

    private void OnNewCheckoutNotification(object sender, (int tableNumber, string zoneName) checkoutInfo)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            PreZoneText.Text = $"Table {checkoutInfo.tableNumber} in zone";
            ZoneText.Text = checkoutInfo.zoneName;
            PostZoneText.Text = "has completed checkout";
            CheckoutNotification.IsOpen = true;
        });
    }
}