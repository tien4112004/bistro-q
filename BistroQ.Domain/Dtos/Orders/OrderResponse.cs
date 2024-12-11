namespace BistroQ.Domain.Dtos.Orders;

public class OrderResponse
{
    public string OrderId { get; set; } = null!;

    public decimal? TotalAmount { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string Status { get; set; }

    public int PeopleCount { get; set; }

    public int? TableId { get; set; }
}
