using BistroQ.Presentation.ViewModels.Models;

namespace BistroQ.Presentation.Messages;

public record AddProductToCartMessage(ProductViewModel Product);

public record OrderRequestedMessage(IEnumerable<OrderItemViewModel> OrderItems);

public record OrderSucceededMessage();

public record CheckoutRequestedMessage(int? TableId);
