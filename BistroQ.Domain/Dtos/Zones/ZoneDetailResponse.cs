using BistroQ.Domain.Dtos.Tables;

namespace BistroQ.Domain.Dtos.Zones;

public class ZoneDetailResponse
{
    public int? ZoneId { get; set; }

    public string? Name { get; set; }
    
    public ICollection<TableResponse> Tables { get; set; }
}