namespace BistroQ.Domain.Models.Entities;

public class OrderItem
{
    public string OrderItemId { get; set; }

    public string? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public string? Status { get; set; }

    public decimal? PriceAtPurchase { get; set; }

    public Product Product { get; set; }
}