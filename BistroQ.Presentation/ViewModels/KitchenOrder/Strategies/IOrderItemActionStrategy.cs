using BistroQ.Presentation.ViewModels.Models;
using BistroQ.Presentation.ViewModels.States;

namespace BistroQ.Presentation.ViewModels.KitchenOrder.Strategies;

/// <summary>
/// Defines the contract for kitchen order item action strategies.
/// Part of the strategy pattern implementation for order item operations.
/// </summary>
public interface IOrderItemActionStrategy
{
    /// <summary>
    /// Gets or sets the state container for kitchen orders
    /// </summary>
    public KitchenOrderState State { get; set; }

    /// <summary>
    /// Executes the strategy's action on the specified order items
    /// </summary>
    /// <param name="orderItems">The collection of order items to process</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public Task ExecuteAsync(IEnumerable<OrderItemViewModel> orderItems);
}