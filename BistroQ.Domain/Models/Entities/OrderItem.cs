using BistroQ.Domain.Enums;

namespace BistroQ.Domain.Models.Entities;

public class OrderItem
{
    public int OrderDetailId { get; set; }

    public string? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public OrderItemStatus? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public decimal? PriceAtPurchase { get; set; }

    public Order? Order { get; set; }

    public Product Product { get; set; }
}