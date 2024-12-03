using BistroQ.Domain.Dtos.Products;

namespace BistroQ.Domain.Dtos.Category;

public class CategoryResponse
{
    public int? CategoryId { get; set; }

    public string? Name { get; set; }

    public List<ProductResponse> Products { get; set; } = new List<ProductResponse>();
}