using BistroQ.Presentation.ViewModels.Models;
using BistroQ.Presentation.ViewModels.States;

namespace BistroQ.Presentation.ViewModels.KitchenOrder.Strategies;

public interface IOrderItemActionStrategy
{
    public KitchenOrderState State { get; set; }
    public void ExecuteAsync(IEnumerable<OrderItemViewModel> orderItems);
}