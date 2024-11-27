﻿using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace BistroQ.ViewModels.Client;

public partial class OrderCartViewModel : ObservableRecipient
{
    private readonly IOrderDataService _orderDataService;

    public Order Order { get; set; }

    [ObservableProperty]
    private bool _isOrdering;

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public ObservableCollection<OrderItem> CartItems { get; set; } = new ObservableCollection<OrderItem>();
    public ObservableCollection<OrderItem> ProcessingItems { get; set; } = new ObservableCollection<OrderItem>();
    public ObservableCollection<OrderItem> CompletedItems { get; set; } = new ObservableCollection<OrderItem>();

    public ICommand StartOrderCommand { get; }
    public ICommand CancelOrderCommand { get; }
    public ICommand OrderStartedCommand { get; set; }

    public OrderCartViewModel(IOrderDataService orderDataService)
    {
        Debug.WriteLine(1);
        _orderDataService = orderDataService;
        StartOrderCommand = new AsyncRelayCommand(StartOrder);
        CancelOrderCommand = new RelayCommand(CancelOrder);
    }

    public void AddProductToCart(Product product)
    {
        var existingOrderItem = CartItems.FirstOrDefault(od => od.ProductId == product.ProductId);
        if (existingOrderItem != null)
        {
            existingOrderItem.Quantity++;
        }
        else
        {
            CartItems.Add(new OrderItem
            {
                OrderId = Order.OrderId,
                ProductId = product.ProductId,
                Product = product,
                Quantity = 1,
                PriceAtPurchase = product.Price
            });
        }
        _ = Task.CompletedTask;
    }

    public async Task LoadExistingOrderAsync()
    {
        IsLoading = true;
        var existingOrder = await Task.Run(async () =>
        {
            return await _orderDataService.GetOrderAsync();
        });

        IsLoading = false;

        if (existingOrder == null)
        {
            return;
        }
        Order = existingOrder;
        IsOrdering = true;
    }

    private async Task StartOrder()
    {
        try
        {
            IsLoading = true;

            await Task.Delay(400); // TODO: This if for UI visualize only, remove it afterward  

            Order = await Task.Run(
                async () =>
                {
                    return await _orderDataService.CreateOrderAsync();
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
}
