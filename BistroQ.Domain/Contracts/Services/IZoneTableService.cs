using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Tables;

namespace BistroQ.Domain.Contracts.Services;

public interface ITableDataService
{
    Task<PaginationResponse<IEnumerable<TableResponse>>> GetGridDataAsync(TableCollectionQueryParams query);

    Task<TableResponse> GetDataTableAsync(int tableId);
    Task<ApiResponse<TableResponse>> CreateTableAsync(CreateTableRequest request);
    Task<ApiResponse<TableResponse>> UpdateTableAsync(int zoneId, UpdateTableRequest request);
    Task<ApiResponse<object>> DeleteTableAsync(int zoneId);

    Task<IEnumerable<TableResponse>> GetTablesByCashierAsync(int zoneId, string type);
}
