namespace BistroQ.Domain.Dtos;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }

    public string Message { get; set; }

    public string Error { get; set; }

    public int StatusCode { get; set; }

    public Pagination Pagination { get; set; }
}
