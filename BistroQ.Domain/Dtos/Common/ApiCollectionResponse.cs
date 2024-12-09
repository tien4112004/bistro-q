namespace BistroQ.Domain.Dtos;

public class ApiCollectionResponse<T>
    where T : class
{
    public ApiCollectionResponse(T data, int totalItems, int page, int totalPages)
    {
        Data = data;
        TotalItems = totalItems;
        CurrentPage = page;
        TotalPages = totalPages;
    }

    public bool Success { get; set; } = true;

    public T Data { get; set; }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalItems { get; set; } = 0;

    public string Message { get; set; }
}