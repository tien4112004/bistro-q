namespace BistroQ.Presentation.Messages;
public record ZoneSelectedMessage(int? ZoneId, string Type);
public record TableSelectedMessage(int? TableId);
public record OrderUpdatedMessage(int? TableId);
public record CheckoutRequestedMessage(int? TableId);
