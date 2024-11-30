using BistroQ.Presentation.Enums;

namespace BistroQ.Presentation.Messages;

public record RemoveOrderItemsMessage(IEnumerable<int> OrderItemIds, KitchenColumnType Source);

public record KitchenActionMessage(IEnumerable<int> OrderItemIds, KitchenAction Action);
