
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using BistroQ.Domain.Models.Entities;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Client;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Presentation.Views.UserControls.Client;

public sealed partial class CartControl : UserControl, INotifyPropertyChanged
{
    public OrderCartViewModel ViewModel { get; }

    public bool CartIsEmpty => ViewModel?.CartItems == null || ViewModel.CartItems.Count == 0;

    public ICommand RemoveProductFromCartCommand { get; }

    public CartControl(OrderCartViewModel orderCartViewModel)
    {
        ViewModel = orderCartViewModel;
        this.InitializeComponent();

        if (ViewModel != null)
        {
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            if (ViewModel.CartItems != null)
            {
                ViewModel.CartItems.CollectionChanged += CartItems_CollectionChanged;
            }
        }

        RemoveProductFromCartCommand = new RelayCommand<OrderItemViewModel>(RemoveProductFromCart);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModel.CartItems))
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CartIsEmpty)));
            if (ViewModel.CartItems != null)
            {
                ViewModel.CartItems.CollectionChanged += CartItems_CollectionChanged;
            }
        }
    }

    private void CartItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CartIsEmpty)));
    }

    private void RemoveProductFromCart(OrderItemViewModel item)
    {
        if (item == null)
        {
            return;
        }
        ViewModel.CartItems.Remove(item);
    }

    private void TableBillSummaryControl_OrderRequested(object sender, EventArgs e)
    {
        if (ViewModel?.CartItems == null || !ViewModel.CartItems.Any())
        {
            Debug.WriteLine("[Debug] No items in the cart to order.");
            return;
        }

        var orderItems = ViewModel.CartItems.ToList();
        App.GetService<IMessenger>().Send(new OrderRequestedMessage(orderItems));
        Debug.WriteLine("[Debug] Order clicked");
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