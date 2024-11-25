using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Domain.Contracts.Services;

public interface IZoneDataService
{
    Task<PaginationResponse<IEnumerable<Zone>>> GetGridDataAsync(ZoneCollectionQueryParams query);
    Task<ApiResponse<Zone>> CreateZoneAsync(CreateZoneRequest request);
    Task<ApiResponse<Zone>> UpdateZoneAsync(int zoneId, UpdateZoneRequest request);
    Task<ApiResponse<object>> DeleteZoneAsync(int zoneId);
    Task<PaginationResponse<IEnumerable<Zone>>> GetZonesAsync(ZoneCollectionQueryParams query);
    Task<Zone> GetZoneByIdAsync(int zoneId);
}