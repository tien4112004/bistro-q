namespace BistroQ.Domain.Dtos.Products;

public class ProductResponse
{
    public int? ProductId { get; set; }

    public string Name { get; set; }

    public int? CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public decimal? Price { get; set; }

    public string Unit { get; set; }

    public decimal? DiscountPrice { get; set; }

    public NutritionFactResponse? NutritionFact { get; set; }

    public string? ImageUrl { get; set; }
}