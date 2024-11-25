namespace BistroQ.Domain.Dtos.Products;

public class ProductResponse
{
    public string? ProductId { get; set; }

    public string Name { get; set; }

    public string? CategoryId { get; set; }

    public decimal? Price { get; set; }

    public string Unit { get; set; }

    public decimal? DiscountPrice { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public string? ImageId { get; set; }
}