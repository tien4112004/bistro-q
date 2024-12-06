using BistroQ.Domain.Contracts.Services;
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

    public async Task ExecuteAsync(IEnumerable<OrderItemViewModel> orderItems)
    {
        var orderItemList = orderItems.ToList();
        await _orderItemDataService.BulkUpdateOrderItemsStatusAsync(orderItems.Select(i => i.OrderItemId), OrderItemStatus.Completed);

        foreach (var item in orderItemList)
        {
            item.Status = OrderItemStatus.Completed;
            State.ProgressItems.Remove(item);
        }
    }
}