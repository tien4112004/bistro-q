namespace BistroQ.Domain.Dtos;

public class ApiCollectionResponse<T>
    where T : class
{
    public ApiCollectionResponse(T data, int totalItems, int page, int size)
    {
        Data = data;
        TotalItems = totalItems;
        CurrentPage = page;
        TotalPages = size != 0 ? (int)Math.Ceiling(totalItems / (double)size) : 0;
    }

    public bool Success { get; set; } = true;

    public T Data { get; set; }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalItems { get; set; } = 0;
}