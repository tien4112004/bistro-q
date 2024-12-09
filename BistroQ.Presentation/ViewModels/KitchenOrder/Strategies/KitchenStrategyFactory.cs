using BistroQ.Domain.Contracts.Services;
using BistroQ.Presentation.Enums;
using BistroQ.Presentation.ViewModels.States;

namespace BistroQ.Presentation.ViewModels.KitchenOrder.Strategies;

public class KitchenStrategyFactory
{
    private readonly Dictionary<KitchenAction, Func<IOrderItemDataService, KitchenOrderState, IOrderItemActionStrategy>> _strategies;
    private readonly IOrderItemDataService _orderItemDataService;

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

    public IOrderItemActionStrategy GetStrategy(KitchenAction action, KitchenOrderState state) =>
        _strategies[action](_orderItemDataService, state);
}