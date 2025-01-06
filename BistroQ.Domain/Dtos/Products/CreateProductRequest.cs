namespace BistroQ.Domain.Dtos.Products;

public class CreateProductRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string Unit { get; set; }
    public int? CategoryId { get; set; }

    public decimal? Calories { get; set; }
    public decimal? Fat { get; set; }
    public decimal? Fiber { get; set; }
    public decimal? Protein { get; set; }
    public decimal? Carbohydrates { get; set; }
}
