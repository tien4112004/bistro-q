using BistroQ.Domain.Enums;
using BistroQ.Presentation.Enums;
using BistroQ.Presentation.ViewModels.Models;

namespace BistroQ.Presentation.Messages;

public record RemoveOrderItemsMessage(IEnumerable<int> OrderItemIds, KitchenColumnType Source);

public record KitchenActionMessage(IEnumerable<OrderItemViewModel> OrderItems, KitchenAction Action);

public record OrderGridStatusChangedMessage(OrderItemStatus Status);