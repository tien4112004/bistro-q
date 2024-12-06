using BistroQ.Domain.Enums;

namespace BistroQ.Domain.Dtos.Orders;

public class UpdateOrderItemStatusRequest
{
    public IEnumerable<int> OrderItemIds { get; set; }

    public OrderItemStatus Status { get; set; }
}
