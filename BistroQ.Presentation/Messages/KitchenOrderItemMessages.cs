using BistroQ.Presentation.Enums;
using BistroQ.Presentation.ViewModels.Models;

namespace BistroQ.Presentation.Messages;

public record RemoveOrderItemsMessage(IEnumerable<int> OrderItemIds, KitchenColumnType Source);

public record KitchenActionMessage(IEnumerable<KitchenOrderItemViewModel> OrderItems, KitchenAction Action);
