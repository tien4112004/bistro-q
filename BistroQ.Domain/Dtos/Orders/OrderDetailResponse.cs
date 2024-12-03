using BistroQ.Domain.Dtos.Orders;

namespace BistroQ.Domain.Dtos.Order;

public class OrderDetailResponse
{
    public string OrderId { get; set; } = null!;
    
    public decimal? TotalAmount { get; set; }
    
    public DateTime? StartTime { get; set; }
    
    public DateTime? EndTime { get; set; }

    public string Status { get; set; }

    public int PeopleCount { get; set; }
    
    public int? TableId { get; set; }
    
    public List<OrderItemDetailResponse> OrderItems { get; set; } = new List<OrderItemDetailResponse>();
}