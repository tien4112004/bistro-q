using BistroQ.Domain.Dtos.Products;

namespace BistroQ.Domain.Dtos.Order;

public class OrderItemDetailResponse
{
    public string OrderItemId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public decimal PriceAtPurchase { get; set; }

    public int Quantity { get; set; }

    public ProductResponse Product { get; set; }
}