using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.ViewModels.Models;
using BistroQ.Presentation.ViewModels.States;

namespace BistroQ.Presentation.ViewModels.KitchenOrder.Strategies;

public class MoveItemStrategy : IOrderItemActionStrategy
{
    private readonly IOrderItemDataService _orderItemDataService;
    public KitchenOrderState State { get; set; }

    public MoveItemStrategy(IOrderItemDataService orderItemDataService, KitchenOrderState state)
    {
        _orderItemDataService = orderItemDataService;
        State = state;
    }

    public async Task ExecuteAsync(IEnumerable<OrderItemViewModel> orderItems)
    {
        var orderItemsList = orderItems.ToList();

        var targetStatus = orderItemsList[0].Status == OrderItemStatus.Pending
            ? OrderItemStatus.InProgress
            : OrderItemStatus.Pending;

        await _orderItemDataService.BulkUpdateOrderItemsStatusAsync(orderItems.Select(x => x.OrderItemId), targetStatus);


        for (var i = 0; i < orderItemsList.Count(); i++)
        {
            orderItemsList[i].Status = targetStatus;
            if (targetStatus == OrderItemStatus.InProgress)
            {
                State.PendingItems.Remove(orderItemsList[i]);
                State.ProgressItems.Add(orderItemsList[i]);
            }
            else
            {
                State.ProgressItems.Remove(orderItemsList[i]);
                State.PendingItems.Add(orderItemsList[i]);
            }
        }
    }
}