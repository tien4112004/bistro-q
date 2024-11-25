using BistroQ.Core.Entities;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BistroQ.Views.Client;

public sealed partial class OrderPage : Page, INotifyCollectionChanged
{
    public ObservableCollection<OrderItem> ProcessingOrders { get; } = new();
    public ObservableCollection<OrderItem> CompletedOrders { get; } = new();

    public bool ProcessingOrdersIsEmpty => ProcessingOrders == null || ProcessingOrders.Count() == 0;
    public bool CompletedOrdersIsEmpty => CompletedOrders == null || CompletedOrders.Count() == 0;

    public OrderPage()
    {
        this.InitializeComponent();
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
}