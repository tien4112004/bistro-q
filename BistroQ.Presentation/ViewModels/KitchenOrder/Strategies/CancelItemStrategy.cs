using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.ViewModels.Models;
using BistroQ.Presentation.ViewModels.States;

namespace BistroQ.Presentation.ViewModels.KitchenOrder.Strategies;

public class CancelItemStrategy : IOrderItemActionStrategy
{
    private readonly IOrderItemDataService _orderItemDataService;
    public KitchenOrderState State { get; set; }

    public CancelItemStrategy(IOrderItemDataService orderItemDataService, KitchenOrderState state)
    {
        _orderItemDataService = orderItemDataService;
        State = state;
    }

    public async Task ExecuteAsync(IEnumerable<OrderItemViewModel> orderItems)
    {
        var orderItemList = orderItems.ToList();

        await _orderItemDataService.BulkUpdateOrderItemsStatusAsync(orderItems.Select(x => x.OrderItemId), OrderItemStatus.Cancelled);

        foreach (var item in orderItemList)
        {
            item.Status = OrderItemStatus.Cancelled;
            State.PendingItems.Remove(item);
        }
    }
}