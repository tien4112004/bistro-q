using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.ViewModels.Models;
using BistroQ.Presentation.ViewModels.States;

namespace BistroQ.Presentation.ViewModels.KitchenOrder.Strategies;

public class MoveItemStrategy: IOrderItemActionStrategy
{
    private readonly IOrderItemDataService _orderItemDataService;
    public KitchenOrderState State { get; set; }

    public MoveItemStrategy(IOrderItemDataService orderItemDataService, KitchenOrderState state)
    {
        _orderItemDataService = orderItemDataService;
        State = state;
    }

    public async void ExecuteAsync(IEnumerable<OrderItemViewModel> orderItems)
    {
        var orderItemViewModels = orderItems as OrderItemViewModel[] ?? orderItems.ToArray();
        var targetStatus = orderItemViewModels.First().Status == OrderItemStatus.Pending
            ? OrderItemStatus.InProgress
            : OrderItemStatus.Pending;
        await Task.WhenAll(
            orderItemViewModels.Select(i => _orderItemDataService.UpdateOrderItemStatusAsync(i.OrderItemId, targetStatus)));

        foreach (var item in orderItemViewModels)
        {
            item.Status = targetStatus;
            if (targetStatus == OrderItemStatus.InProgress)
            {
                State.PendingItems.Remove(item);
                State.ProgressItems.Add(item);
            }
            else
            {
                State.ProgressItems.Remove(item);
                State.PendingItems.Add(item);
            }
        }
    }
}