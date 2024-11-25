using BistroQ.Domain.Dtos.Tables;

namespace BistroQ.Domain.Dtos.Zones;

public class ZoneDetailDto
{
    public int? ZoneId { get; set; }

    public string? Name { get; set; }
    public ICollection<TableDto> Tables { get; set; }
}
