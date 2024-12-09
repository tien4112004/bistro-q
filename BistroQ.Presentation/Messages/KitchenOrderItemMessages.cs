using BistroQ.Domain.Enums;
using BistroQ.Presentation.Enums;

namespace BistroQ.Presentation.Messages;

public record RemoveOrderItemsMessage(IEnumerable<int> OrderItemIds, KitchenColumnType Source);

public record KitchenActionMessage(KitchenAction Action);

public record OrderGridStatusChangedMessage(OrderItemStatus Status);