namespace BistroQ.Domain.Models.Entities;

public class Product
{
    public int? ProductId { get; set; }

    public string Name { get; set; }

    public int? CategoryId { get; set; }

    public Category Category { get; set; }

    public decimal? Price { get; set; }

    public string Unit { get; set; }

    public decimal? DiscountPrice { get; set; }

    public string? ImageUrl { get; set; }

    public NutritionFact? NutritionFact { get; set; }
}