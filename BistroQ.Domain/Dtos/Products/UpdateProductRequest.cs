namespace BistroQ.Domain.Dtos.Products;

public class UpdateProductRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string Unit { get; set; }
    public int? CategoryId { get; set; }
    public NutritionFactResponse? NutritionFact { get; set; }
}
