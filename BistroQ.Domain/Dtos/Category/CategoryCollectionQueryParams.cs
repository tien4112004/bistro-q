namespace BistroQ.Domain.Dtos.Category;

public class CategoryCollectionQueryParams : BaseCollectionQueryParams
{
    public int? CategoryId { get; set; }

    public string? Name { get; set; }
}
