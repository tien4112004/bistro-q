using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using System.Diagnostics;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Client;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Presentation.Views.UserControls.Client;

public sealed partial class OrderControl : UserControl, INotifyPropertyChanged
{
    public OrderCartViewModel ViewModel { get; }

    public bool ProcessingOrdersIsEmpty => ViewModel.ProcessingItems == null || ViewModel.ProcessingItems.Count() == 0;
    public bool CompletedOrdersIsEmpty => ViewModel.CompletedItems == null || ViewModel.CompletedItems.Count() == 0;

    public OrderControl(OrderCartViewModel orderCartViewModel)
    {
        this.InitializeComponent();
        ViewModel = orderCartViewModel;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void TableBillSummaryControl_CheckoutRequested(object sender, EventArgs e)
    {
        App.GetService<IMessenger>().Send(new CheckoutRequestedMessage(ViewModel.Order.TableId));
        Debug.WriteLine("[Debug] Checkout clicked");
    }
    
    private void VerticalScrollViewer_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
    {
        const double SCROLL_SPEED = 1.25;
        var scrollViewer = (ScrollViewer)sender;
        scrollViewer.ChangeView(
            null,
            scrollViewer.VerticalOffset - e.Delta.Translation.Y * SCROLL_SPEED,
            null,
            true);
    }
}