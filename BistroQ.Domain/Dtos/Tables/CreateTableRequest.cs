namespace BistroQ.Domain.Dtos.Tables;

public class CreateTableRequest
{
    public int ZoneId { get; set; }
    public int? SeatsCount { get; set; }
}