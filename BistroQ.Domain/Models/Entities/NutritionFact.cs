using BistroQ.Domain.Models.Entities;

namespace BistroQ.Domain.Models.Entities;

public class NutritionFact
{
    public int ProductId { get; set; }

    public double? Calories { get; set; }

    public virtual Product Product { get; set; } = null!;
}