using BistroQ.Presentation.Enums;

namespace BistroQ.Presentation.Messages;
public record ZoneSelectedMessage(int? ZoneId, string Type);
public record TableSelectedMessage(int? TableId);
public record OrderUpdatedMessage(int? TableId);
public record CompleteCheckoutRequestedMessage(int? TableId);
public record TableStateChangedMessage(int? TableId, CashierTableState State);
