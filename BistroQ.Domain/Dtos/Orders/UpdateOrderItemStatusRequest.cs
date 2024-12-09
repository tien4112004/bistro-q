namespace BistroQ.Domain.Dtos.Orders;

public class UpdateOrderItemStatusRequest
{
    public IEnumerable<string> OrderItemIds { get; set; }

    public string Status { get; set; }
}
