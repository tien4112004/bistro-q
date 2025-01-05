using BistroQ.Presentation.ViewModels.Models;

namespace BistroQ.Presentation.Messages;

/// <summary>
/// Record representing a message sent when a product is added to the shopping cart.
/// </summary>
/// <param name="Product">The product view model to be added to the cart.</param>
public record AddProductToCartMessage(ProductViewModel Product);

/// <summary>
/// Record representing a message sent when an order is requested to be placed.
/// </summary>
/// <param name="OrderItems">Collection of order items to be processed in the order.</param>
public record OrderRequestedMessage(IEnumerable<OrderItemViewModel> OrderItems);

/// <summary>
/// Record representing a message sent when an order has been successfully processed.
/// Contains no parameters as it's used purely as a notification signal.
/// </summary>
public record OrderSucceededMessage();

/// <summary>
/// Record representing a message sent when a checkout is requested for a table.
/// </summary>
/// <param name="TableId">The ID of the table requesting checkout. Null if not associated with a specific table.</param>
public record CheckoutRequestedMessage(int? TableId);