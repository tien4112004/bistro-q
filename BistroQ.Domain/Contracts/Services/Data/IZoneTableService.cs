using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Tables;

namespace BistroQ.Domain.Contracts.Services;

public interface ITableDataService
{
    Task<PaginationResponse<IEnumerable<Table>>> GetGridDataAsync(TableCollectionQueryParams query);

    Task<Table> GetDataTableAsync(int tableId);
    Task<Table> CreateTableAsync(CreateTableRequest request);
    Task<Table> UpdateTableAsync(int zoneId, UpdateTableRequest request);
    Task DeleteTableAsync(int zoneId);

    Task<IEnumerable<Table>> GetTablesByCashierAsync(int zoneId, string type);
}