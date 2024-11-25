using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Zones;

namespace BistroQ.Domain.Contracts.Services;

public interface IZoneDataService
{
    Task<PaginationResponse<IEnumerable<ZoneResponse>>> GetGridDataAsync(ZoneCollectionQueryParams query);
    Task<ApiResponse<ZoneResponse>> CreateZoneAsync(CreateZoneRequest request);
    Task<ApiResponse<ZoneResponse>> UpdateZoneAsync(int zoneId, UpdateZoneRequest request);
    Task<ApiResponse<object>> DeleteZoneAsync(int zoneId);
    Task<PaginationResponse<IEnumerable<ZoneResponse>>> GetZonesAsync(ZoneCollectionQueryParams query);
    Task<ZoneResponse> GetZoneByIdAsync(int zoneId);
}