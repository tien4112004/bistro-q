using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.ViewModels.Models;
using BistroQ.Presentation.ViewModels.States;

namespace BistroQ.Presentation.ViewModels.KitchenOrder.Strategies;

/// <summary>
/// Strategy implementation for completing order items.
/// Handles the completion process and updates item status accordingly.
/// </summary>
public class CompleteItemStrategy : IOrderItemActionStrategy
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
    /// Initializes a new instance of the CompleteItemStrategy class
    /// </summary>
    /// <param name="orderItemDataService">Service for order item operations</param>
    /// <param name="state">The current kitchen order state</param>
    public CompleteItemStrategy(IOrderItemDataService orderItemDataService, KitchenOrderState state)
    {
        _orderItemDataService = orderItemDataService;
        State = state;
    }

    /// <summary>
    /// Executes the completion strategy on the specified order items
    /// </summary>
    /// <remarks>
    /// Updates the status of items to Completed and removes them from the in-progress items collection.
    /// </remarks>
    /// <param name="orderItems">The collection of order items to complete</param>
    /// <returns>A task representing the asynchronous operation</returns>
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