namespace BistroQ.Domain.Models.Entities;

public class NutritionFact
{
    public int ProductId { get; set; }

    public decimal? Calories { get; set; }
    public decimal? Fat { get; set; }
    public decimal? Fiber { get; set; }
    public decimal? Protein { get; set; }
    public decimal? Carbohydrates { get; set; }

    public virtual Product Product { get; set; } = null!;
}