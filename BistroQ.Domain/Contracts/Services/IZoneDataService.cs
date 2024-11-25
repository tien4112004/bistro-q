using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Zones;

namespace BistroQ.Domain.Contracts.Services;

public interface IZoneDataService
{
    Task<PaginationResponseDto<IEnumerable<ZoneDto>>> GetGridDataAsync(ZoneCollectionQueryParams query);
    Task<ApiResponse<ZoneDto>> CreateZoneAsync(CreateZoneRequestDto request);
    Task<ApiResponse<ZoneDto>> UpdateZoneAsync(int zoneId, UpdateZoneRequestDto request);
    Task<ApiResponse<object>> DeleteZoneAsync(int zoneId);
    Task<PaginationResponseDto<IEnumerable<ZoneDto>>> GetZonesAsync(ZoneCollectionQueryParams query);
    Task<ZoneDto> GetZoneByIdAsync(int zoneId);
}
