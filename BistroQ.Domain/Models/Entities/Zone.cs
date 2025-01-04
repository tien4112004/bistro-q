namespace BistroQ.Domain.Models.Entities;

public class Zone
{
    public int? ZoneId { get; set; }

    public string? Name { get; set; }

    public ICollection<Table>? Tables { get; set; }

    public bool? HasCheckingOutTables { get; set; }
}