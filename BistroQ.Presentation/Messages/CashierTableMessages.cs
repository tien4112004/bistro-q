using BistroQ.Presentation.Enums;

namespace BistroQ.Presentation.Messages;

/// <summary>
/// Record representing a message sent when a zone is selected.
/// </summary>
/// <param name="ZoneId">The ID of the selected zone. Null if no zone is selected.</param>
/// <param name="Type">The type or context of the zone selection.</param>
public record ZoneSelectedMessage(int? ZoneId, string Type);

/// <summary>
/// Record representing a message sent when a table is selected.
/// </summary>
/// <param name="TableId">The ID of the selected table. Null if no table is selected.</param>
public record TableSelectedMessage(int? TableId);

/// <summary>
/// Record representing a message sent when an order is updated.
/// </summary>
/// <param name="TableId">The ID of the table whose order was updated. Null if not associated with a specific table.</param>
public record OrderUpdatedMessage(int? TableId);

/// <summary>
/// Record representing a message sent when a checkout is requested for completion.
/// </summary>
/// <param name="TableId">The ID of the table requesting checkout completion. Null if not associated with a specific table.</param>
public record CompleteCheckoutRequestedMessage(int? TableId);

/// <summary>
/// Record representing a message sent when a table's state changes. 
/// </summary>
/// <param name="TableId">The ID of the table whose state changed. Null if not associated with a specific table.</param>
/// <param name="State">The new state of the table in the cashier interface.</param>
public record TableStateChangedMessage(int? TableId, CashierTableState State);

/// <summary>
/// Record representing a message sent when a zone's state changes regarding checkout status. 
/// </summary>
/// <param name="ZoneName">The name of the zone whose state changed.</param>
/// <param name="HasCheckingoutTables">Boolean indicating whether the zone has any tables in checkout process.</param>
public record ZoneStateChangedMessage(string ZoneName, bool HasCheckingoutTables);