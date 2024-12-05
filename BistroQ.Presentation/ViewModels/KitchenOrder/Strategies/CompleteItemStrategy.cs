﻿using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.ViewModels.Models;
using BistroQ.Presentation.ViewModels.States;

namespace BistroQ.Presentation.ViewModels.KitchenOrder.Strategies;

public class CompleteItemStrategy : IOrderItemActionStrategy
{
    private readonly IOrderItemDataService _orderItemDataService;
    public KitchenOrderState State { get; set; }

    public CompleteItemStrategy(IOrderItemDataService orderItemDataService, KitchenOrderState state)
    {
        _orderItemDataService = orderItemDataService;
        State = state;
    }

    public async void ExecuteAsync(IEnumerable<OrderItemViewModel> orderItems)
    {
        var orderItemViewModels = orderItems as OrderItemViewModel[] ?? orderItems.ToArray();
        await Task.WhenAll(
            orderItemViewModels.Select(i =>
                _orderItemDataService.UpdateOrderItemStatusAsync(i.OrderItemId, OrderItemStatus.Completed)));

        foreach (var item in orderItemViewModels)
        {
            item.Status = OrderItemStatus.Completed;
            State.ProgressItems.Remove(item);
        }
    }
}