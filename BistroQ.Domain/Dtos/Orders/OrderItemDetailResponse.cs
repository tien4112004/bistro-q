using BistroQ.Domain.Dtos.Products;

namespace BistroQ.Domain.Dtos.Order;

public class OrderItemDetailResponse
{
    public string OrderItemId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public decimal TotalAmount { get; set; }
    
    public ProductResponse Product { get; set; }
}