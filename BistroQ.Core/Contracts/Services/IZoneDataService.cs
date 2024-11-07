using BistroQ.Core.Dtos;
using BistroQ.Core.Dtos.Zones;

namespace BistroQ.Core.Contracts.Services;

public interface IZoneDataService
{
    Task<PaginationResponseDto<IEnumerable<ZoneDto>>> GetGridDataAsync(ZoneCollectionQueryParams query);
    Task<ApiResponse<ZoneDto>> CreateZoneAsync(CreateZoneRequestDto request);
    Task<ApiResponse<ZoneDto>> UpdateZoneAsync(int zoneId, UpdateZoneRequestDto request);
    Task<ApiResponse<object>> DeleteZoneAsync(int zoneId);
    Task<IEnumerable<ZoneDetailDto>> GetZonesAsync(ZoneCollectionQueryParams query);
    Task<ZoneDetailDto> GetZoneByIdAsync(int zoneId);
}
