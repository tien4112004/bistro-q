using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Tables;

namespace BistroQ.Domain.Contracts.Services;

/// <summary>
/// Interface for managing table-related data operations through API endpoints.
/// Provides methods for CRUD operations on restaurant tables, including admin and cashier-specific endpoints.
/// </summary>
public interface ITableDataService
{
    /// <summary>
    /// Retrieves a paginated collection of tables for admin grid view.
    /// </summary>
    /// <param name="query">Query parameters for filtering, sorting, and pagination</param>
    /// <returns>A collection response containing tables and pagination information</returns>
    /// <exception cref="Exception">Thrown with message "Failed to get tables" when the API request fails</exception>
    Task<ApiCollectionResponse<IEnumerable<Table>>> GetGridDataAsync(TableCollectionQueryParams query);

    /// <summary>
    /// Retrieves details for a specific table.
    /// </summary>
    /// <param name="tableId">The unique identifier of the table</param>
    /// <returns>The table details</returns>
    /// <exception cref="Exception">Thrown with message "Failed to get table" when the API request fails</exception>
    Task<Table> GetDataTableAsync(int tableId);

    /// <summary>
    /// Creates a new table in the system.
    /// </summary>
    /// <param name="request">The table creation details</param>
    /// <returns>The newly created table</returns>
    /// <exception cref="Exception">Thrown with message "Failed to create table" when the API request fails</exception>
    Task<Table> CreateTableAsync(CreateTableRequest request);

    /// <summary>
    /// Updates an existing table's details.
    /// </summary>
    /// <param name="zoneId">The unique identifier of the zone containing the table</param>
    /// <param name="request">The table update details</param>
    /// <returns>The updated table</returns>
    /// <exception cref="Exception">Thrown with message "Failed to update table" when the API request fails</exception>
    Task<Table> UpdateTableAsync(int zoneId, UpdateTableRequest request);

    /// <summary>
    /// Deletes a table from the system.
    /// </summary>
    /// <param name="zoneId">The unique identifier of the zone containing the table</param>
    /// <returns>True if the deletion was successful</returns>
    /// <exception cref="Exception">Thrown with the API error message when the request fails</exception>
    Task<bool> DeleteTableAsync(int zoneId);

    /// <summary>
    /// Retrieves tables in a specific zone based on occupancy status (Cashier endpoint).
    /// </summary>
    /// <param name="zoneId">The unique identifier of the zone</param>
    /// <param name="type">Filter type: "All" for all tables, any other value for occupied tables only</param>
    /// <returns>Collection of tables matching the criteria</returns>
    /// <exception cref="Exception">Thrown with message "Failed to get tables" when the API request fails</exception>
    Task<IEnumerable<Table>> GetTablesByCashierAsync(int zoneId, string type);
}