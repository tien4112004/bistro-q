using BistroQ.Domain.Enums;
using BistroQ.Presentation.Enums;

namespace BistroQ.Presentation.Messages;

/// <summary>
/// Record representing a message to remove order items from a kitchen column.
/// Used in the kitchen interface to manage order item removals.
/// </summary>
/// <param name="OrderItemIds">Collection of order item IDs to be removed.</param>
/// <param name="Source">The type of kitchen column from which the items should be removed.</param>
public record RemoveOrderItemsMessage(IEnumerable<string> OrderItemIds, KitchenColumnType Source);

/// <summary>
/// Record representing a message to execute a kitchen-related action.
/// Used to trigger specific operations in the kitchen workflow.
/// </summary>
/// <param name="Action">The kitchen action to be performed.</param>
/// <remarks>
/// This message is typically used for actions such as starting preparation,
/// marking orders as ready, or handling other kitchen workflow states.
/// </remarks>
public record KitchenActionMessage(KitchenAction Action);

/// <summary>
/// Record representing a message sent when the status of orders in the grid changes.
/// Used to notify components about updates to order status.
/// </summary>
/// <param name="Status">The new status of the orders in the grid.</param>
public record OrderGridStatusChangedMessage(OrderItemStatus Status);