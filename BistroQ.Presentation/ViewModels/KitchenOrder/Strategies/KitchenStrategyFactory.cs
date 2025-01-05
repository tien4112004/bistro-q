using BistroQ.Domain.Contracts.Services;
using BistroQ.Presentation.Enums;
using BistroQ.Presentation.ViewModels.States;

namespace BistroQ.Presentation.ViewModels.KitchenOrder.Strategies;

/// <summary>
/// Factory class responsible for creating appropriate order item action strategies.
/// Implements the factory pattern to instantiate different strategy implementations
/// based on the requested kitchen action.
/// </summary>
public class KitchenStrategyFactory
{
    /// <summary>
    /// Dictionary mapping kitchen actions to their corresponding strategy creation functions
    /// </summary>
    private readonly Dictionary<KitchenAction, Func<IOrderItemDataService, KitchenOrderState, IOrderItemActionStrategy>> _strategies;

    /// <summary>
    /// Service for order item data operations
    /// </summary>
    private readonly IOrderItemDataService _orderItemDataService;

    /// <summary>
    /// Initializes a new instance of the KitchenStrategyFactory class
    /// </summary>
    /// <param name="service">Service for order item operations</param>
    public KitchenStrategyFactory(IOrderItemDataService service)
    {
        _orderItemDataService = service;
        _strategies = new Dictionary<KitchenAction, Func<IOrderItemDataService, KitchenOrderState, IOrderItemActionStrategy>>
        {
            [KitchenAction.Complete] = (s, state) => new CompleteItemStrategy(s, state),
            [KitchenAction.Move] = (s, state) => new MoveItemStrategy(s, state),
            [KitchenAction.Cancel] = (s, state) => new CancelItemStrategy(s, state)
        };
    }

    /// <summary>
    /// Creates and returns the appropriate strategy for the specified kitchen action
    /// </summary>
    /// <param name="action">The kitchen action to create a strategy for</param>
    /// <param name="state">The current kitchen order state</param>
    /// <returns>An implementation of IOrderItemActionStrategy</returns>
    public IOrderItemActionStrategy GetStrategy(KitchenAction action, KitchenOrderState state) =>
        _strategies[action](_orderItemDataService, state);
}