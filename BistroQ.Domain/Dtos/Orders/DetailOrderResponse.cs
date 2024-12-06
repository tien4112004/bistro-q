using BistroQ.Domain.Dtos.Orders;

namespace BistroQ.Domain.Dtos.Order;

public class DetailOrderResponse
{
    public string OrderId { get; set; } = null!;

    public decimal? TotalAmount { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string Status { get; set; }

    public int PeopleCount { get; set; }

    public int? TableId { get; set; }

    public List<OrderItemWithProductResponse> OrderItems { get; set; } = new List<OrderItemWithProductResponse>();
}