namespace BistroQ.Domain.Dtos;

public class BaseCollectionQueryParams
{
    public int Page { get; set; } = 1;

    public int Size { get; set; } = 10;

    public string? OrderBy { get; set; }

    public string? OrderDirection { get; set; } = "asc";
}
