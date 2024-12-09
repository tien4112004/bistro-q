using BistroQ.Domain.Dtos.Products;
using BistroQ.Domain.Dtos.Tables;

namespace BistroQ.Domain.Dtos.Orders;

public class DetailOrderItemResponse
{
    public string? OrderItemId { get; set; } = null!;

    public int? ProductId { get; set; } = null!;

    public decimal PriceAtPurchase { get; set; }

    public int Quantity { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public TableResponse Table { get; set; } = null!;

    public OrderResponse Order { get; set; } = null!;

    public ProductResponse Product { get; set; }
}
