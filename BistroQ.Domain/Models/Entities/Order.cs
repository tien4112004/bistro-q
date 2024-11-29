namespace BistroQ.Domain.Models.Entities;

public class Order
{
    public string OrderId { get; set; } = null!;

    public decimal? TotalAmount { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string Status { get; set; }

    public int PeopleCount { get; set; }

    public int? TableId { get; set; }

    public Table? Table { get; set; } = null;

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}