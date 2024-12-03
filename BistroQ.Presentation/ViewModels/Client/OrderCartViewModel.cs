﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace BistroQ.Presentation.ViewModels.Client;

public partial class OrderCartViewModel : 
    ObservableRecipient, 
    IRecipient<AddProductToCartMessage>, 
    IRecipient<OrderRequestedMessage>,
    IRecipient<CheckoutRequestedMessage>,
    IDisposable
{
    private readonly IOrderDataService _orderDataService;
    private readonly IMapper _mapper;
    private readonly IMessenger _messenger;

    public OrderViewModel Order { get; set; }

    [ObservableProperty]
    private bool _isOrdering;

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public decimal TotalCart => CartItems.Sum(x => x.Total.Value);

    public ObservableCollection<OrderItemViewModel> CartItems { get; set; } = new ObservableCollection<OrderItemViewModel>();
    public ObservableCollection<OrderItemViewModel> ProcessingItems { get; set; } = new ObservableCollection<OrderItemViewModel>();
    public ObservableCollection<OrderItemViewModel> CompletedItems { get; set; } = new ObservableCollection<OrderItemViewModel>();

    public ICommand StartOrderCommand { get; }
    public ICommand CancelOrderCommand { get; }
    public ICommand OrderStartedCommand { get; set; }

    public OrderCartViewModel(IOrderDataService orderDataService, IMapper mapper, IMessenger messenger)
    {
        _orderDataService = orderDataService;
        _mapper = mapper;
        _messenger = messenger;
        messenger.RegisterAll(this);
        StartOrderCommand = new AsyncRelayCommand(StartOrder);
        CancelOrderCommand = new RelayCommand(CancelOrder);

        CartItems.CollectionChanged += CartItems_CollectionChanged;
    }

    public async Task LoadExistingOrderAsync()
    {
        IsLoading = true;
        var existingOrder = await Task.Run(async () =>
        {
            var order = await _orderDataService.GetOrderAsync();
            return _mapper.Map<OrderViewModel>(order);
        });

        IsLoading = false;

        if (existingOrder == null)
        {
            return;
        }
        Order = existingOrder;
        IsOrdering = true;

        SeparateOrdersByStatus();
    }

    private async Task StartOrder()
    {
        try
        {
            IsLoading = true;

            Order = await Task.Run(
                async () =>
                {
                    var orders = await _orderDataService.CreateOrderAsync();
                    return _mapper.Map<OrderViewModel>(orders);
                });
            IsOrdering = true;

            OrderStartedCommand?.Execute(Order);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void CancelOrder()
    {
        _orderDataService.DeleteOrderAsync();
        Order = null;

        ErrorMessage = string.Empty;
        IsOrdering = false;
    }

    // CartItemChanged
    private void CartItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (OrderItemViewModel item in e.NewItems)
            {
                item.PropertyChanged += OrderItem_PropertyChanged;
            }
        }

        if (e.OldItems != null)
        {
            foreach (OrderItemViewModel item in e.OldItems)
            {
                item.PropertyChanged -= OrderItem_PropertyChanged;
            }
        }

        OnPropertyChanged(nameof(TotalCart));
        SeparateOrdersByStatus();
    }

    private void OrderItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(OrderItemViewModel.Total))
        {
            OnPropertyChanged(nameof(TotalCart));
        }
    }

    private void SeparateOrdersByStatus()
    {
        ProcessingItems.Clear();
        CompletedItems.Clear();

        foreach (var item in Order.OrderItems)
        {
            if (item.Status == "Processing")
            {
                ProcessingItems.Add(item);
            }
            else if (item.Status == "Completed")
            {
                CompletedItems.Add(item);
            }
        }
        
        OnPropertyChanged(nameof(IsProcessingItemsEmpty));
        OnPropertyChanged(nameof(IsCompletedItemsEmpty));
    }

    public bool IsProcessingItemsEmpty => !ProcessingItems.Any();
    public bool IsCompletedItemsEmpty => !CompletedItems.Any();
    public void Receive(AddProductToCartMessage message)
    {
        var product = message.Product;
        var existingOrderItem = CartItems.FirstOrDefault(od => od.ProductId == product.ProductId);
        if (existingOrderItem != null)
        {
            existingOrderItem.Quantity++;
        }
        else
        {
            CartItems.Add(new OrderItemViewModel
            {
                OrderId = Order.OrderId,
                ProductId = product.ProductId,
                Product = product,
                Quantity = 1,
                PriceAtPurchase = product.Price
            });
        }
    }
    
    public void Receive(OrderRequestedMessage message)
    {
        Debug.WriteLine("[Debug] Order requested message received, number of items: " + message.OrderItems.Count());
    }
    
    public void Receive(CheckoutRequestedMessage message)
    {
        Debug.WriteLine("[Debug] Checkout requested message received, table to be checked out: " + message.TableId);
    }

    public void Dispose()
    {
        _messenger.UnregisterAll(this);
    }
}