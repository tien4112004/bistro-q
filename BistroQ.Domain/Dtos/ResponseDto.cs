namespace BistroQ.Domain.Dtos;

public class ResponseDto<T>
    where T : class
{
    public ResponseDto(T data)
    {
        Success = true;
        Data = data;
    }

    public bool Success
    {
        get; set;
    }

    public T Data
    {
        get; set;
    }
}
