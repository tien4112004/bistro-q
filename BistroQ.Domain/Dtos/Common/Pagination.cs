namespace BistroQ.Domain.Dtos.Common;

public class Pagination
{
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
}
