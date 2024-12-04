using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Domain.Contracts.Services;

public interface IZoneDataService
{
    Task<ApiCollectionResponse<IEnumerable<Zone>>> GetGridDataAsync(ZoneCollectionQueryParams query);
    Task<Zone> CreateZoneAsync(CreateZoneRequest request);
    Task<Zone> UpdateZoneAsync(int zoneId, UpdateZoneRequest request);
    Task<bool> DeleteZoneAsync(int zoneId);
    Task<ApiCollectionResponse<IEnumerable<Zone>>> GetZonesAsync(ZoneCollectionQueryParams query);
    Task<Zone> GetZoneByIdAsync(int zoneId);
}