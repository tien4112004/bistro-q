using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Domain.Contracts.Services;

/// <summary>
/// Interface for managing zone-related data operations through API endpoints.
/// Provides methods for CRUD operations on restaurant zones, with specific endpoints for admin and cashier access.
/// </summary>
public interface IZoneDataService
{
    /// <summary>
    /// Retrieves a paginated collection of zones for admin grid view.
    /// </summary>
    /// <param name="query">Query parameters for filtering, sorting, and pagination</param>
    /// <returns>A collection response containing zone responses and pagination information</returns>
    /// <exception cref="Exception">Thrown with message "Failed to get zones" when the API request fails</exception>
    Task<ApiCollectionResponse<IEnumerable<ZoneResponse>>> GetGridDataAsync(ZoneCollectionQueryParams query);

    /// <summary>
    /// Creates a new zone in the system.
    /// </summary>
    /// <param name="request">The zone creation details</param>
    /// <returns>The newly created zone</returns>
    /// <exception cref="Exception">Thrown with the API error message when the request fails</exception>
    Task<Zone> CreateZoneAsync(CreateZoneRequest request);

    /// <summary>
    /// Updates an existing zone's details.
    /// </summary>
    /// <param name="zoneId">The unique identifier of the zone to update</param>
    /// <param name="request">The zone update details</param>
    /// <returns>The updated zone</returns>
    /// <exception cref="Exception">Thrown with the API error message when the request fails</exception>
    Task<Zone> UpdateZoneAsync(int zoneId, UpdateZoneRequest request);

    /// <summary>
    /// Deletes a zone from the system.
    /// </summary>
    /// <param name="zoneId">The unique identifier of the zone to delete</param>
    /// <returns>True if the deletion was successful</returns>
    /// <exception cref="Exception">Thrown with the API error message when the request fails</exception>
    Task<bool> DeleteZoneAsync(int zoneId);

    /// <summary>
    /// Retrieves a paginated collection of zones for general access.
    /// </summary>
    /// <param name="query">Query parameters for filtering, sorting, and pagination</param>
    /// <returns>A collection response containing zones and pagination information</returns>
    /// <exception cref="Exception">Thrown with message "Failed to get zones" when the API request fails</exception>
    Task<ApiCollectionResponse<IEnumerable<Zone>>> GetZonesAsync(ZoneCollectionQueryParams query);

    /// <summary>
    /// Retrieves details for a specific zone.
    /// </summary>
    /// <param name="zoneId">The unique identifier of the zone</param>
    /// <returns>The zone details</returns>
    /// <exception cref="Exception">Thrown with the API error message when the request fails</exception>
    Task<Zone> GetZoneByIdAsync(int zoneId);

    /// <summary>
    /// Retrieves a paginated collection of zones accessible to cashiers.
    /// </summary>
    /// <param name="query">Optional query parameters for filtering, sorting, and pagination</param>
    /// <returns>A collection response containing zones and pagination information</returns>
    /// <exception cref="Exception">Thrown with message "Failed to get zones" when the API request fails</exception>
    Task<ApiCollectionResponse<IEnumerable<Zone>>> GetZonesByCashierAsync(ZoneCollectionQueryParams query = null);
}