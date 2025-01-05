using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.ViewModels.Models;
using BistroQ.Presentation.ViewModels.States;

namespace BistroQ.Presentation.ViewModels.KitchenOrder.Strategies;

/// <summary>
/// Strategy implementation for cancelling order items.
/// Handles the cancellation process and updates item status accordingly.
/// </summary>
public class CancelItemStrategy : IOrderItemActionStrategy
{
    /// <summary>
    /// Service for order item data operations
    /// </summary>
    private readonly IOrderItemDataService _orderItemDataService;

    /// <summary>
    /// Gets or sets the state container for kitchen orders
    /// </summary>
    public KitchenOrderState State { get; set; }

    /// <summary>
    /// Initializes a new instance of the CancelItemStrategy class
    /// </summary>
    /// <param name="orderItemDataService">Service for order item operations</param>
    /// <param name="state">The current kitchen order state</param>
    public CancelItemStrategy(IOrderItemDataService orderItemDataService, KitchenOrderState state)
    {
        _orderItemDataService = orderItemDataService;
        State = state;
    }

    /// <summary>
    /// Executes the cancellation strategy on the specified order items
    /// </summary>
    /// <remarks>
    /// Updates the status of items to Cancelled and removes them from the pending items collection.
    /// </remarks>
    /// <param name="orderItems">The collection of order items to cancel</param>
    /// <returns>A task representing the asynchronous operation</returns>
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