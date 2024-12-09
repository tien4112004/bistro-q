namespace BistroQ.Domain.Dtos.Orders;

public class OrderItemColletionQueryParams : BaseCollectionQueryParams
{
    public string? OrderId { get; set; }
    public int? ProductId { get; set; }
    public int? Quantity { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public decimal? PriceAtPurchase { get; set; }
}
