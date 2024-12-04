using BistroQ.Domain.Dtos.Products;

namespace BistroQ.Domain.Dtos.Orders;

public class OrderItemDetailResponse
{
    public string? OrderItemId { get; set; } = null!;

    public int? ProductId { get; set; } = null!;

    public decimal PriceAtPurchase { get; set; }

    public int Quantity { get; set; }

    public string Status { get; set; } = null!;
    public ProductResponse Product { get; set; }
}