using BistroQ.Presentation.Enums;

namespace BistroQ.Presentation.Messages;

public record RemoveOrderItemsMessage(IEnumerable<int> OrderItemIds, KitchenColumnType Source);

public record ChangeColumnMessage(KitchenColumnType Destination);