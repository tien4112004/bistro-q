namespace BistroQ.Domain.Dtos.Tables;
public class TableResponse
{
    public int? TableId { get; set; }

    public int? ZoneId { get; set; }

    public int? Number { get; set; }

    public int? SeatsCount { get; set; }

    public string? ZoneName { get; set; }

    public bool IsOccupied { get; set; }

    public bool IsCheckingOut { get; set; }
}