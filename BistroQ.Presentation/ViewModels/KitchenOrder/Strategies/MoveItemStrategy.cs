using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.ViewModels.Models;
using BistroQ.Presentation.ViewModels.States;

namespace BistroQ.Presentation.ViewModels.KitchenOrder.Strategies;

/// <summary>
/// Strategy implementation for moving order items between states.
/// Handles transitions between Pending and InProgress states.
/// </summary>
public class MoveItemStrategy : IOrderItemActionStrategy
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
    /// Initializes a new instance of the MoveItemStrategy class
    /// </summary>
    /// <param name="orderItemDataService">Service for order item operations</param>
    /// <param name="state">The current kitchen order state</param>
    public MoveItemStrategy(IOrderItemDataService orderItemDataService, KitchenOrderState state)
    {
        _orderItemDataService = orderItemDataService;
        State = state;
    }

    /// <summary>
    /// Executes the move strategy on the specified order items
    /// </summary>
    /// <remarks>
    /// Determines the target status based on the current status of the first item.
    /// Moves items between pending and in-progress collections based on the target status.
    /// </remarks>
    /// <param name="orderItems">The collection of order items to move</param>
    /// <returns>A task representing the asynchronous operation</returns>
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