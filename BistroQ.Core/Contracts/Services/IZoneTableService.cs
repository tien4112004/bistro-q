using BistroQ.Core.Dtos;
using BistroQ.Core.Dtos.Tables;

namespace BistroQ.Core.Contracts.Services;

public interface ITableDataService
{
    Task<PaginationResponseDto<IEnumerable<TableDto>>> GetGridDataAsync(TableCollectionQueryParams query);

    Task<TableDto> GetDataTableAsync(int tableId);
    Task<ApiResponse<TableDto>> CreateTableAsync(CreateTableRequestDto request);
    Task<ApiResponse<TableDto>> UpdateTableAsync(int zoneId, UpdateTableRequestDto request);
    Task<ApiResponse<object>> DeleteTableAsync(int zoneId);
}
