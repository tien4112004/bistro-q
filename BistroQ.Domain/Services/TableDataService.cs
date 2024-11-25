using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Tables;

namespace BistroQ.Domain.Services
{
    public class TableDataService : ITableDataService
    {
        private readonly IApiClient _apiClient;

        public TableDataService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<PaginationResponseDto<IEnumerable<TableDto>>> GetGridDataAsync(TableCollectionQueryParams query = null)
        {
            var response = await _apiClient.GetCollectionAsync<IEnumerable<TableDto>>("/api/admin/table", query);
            return response;
        }

        public async Task<TableDto> GetDataTableAsync(int tableId)
        {
            var response = await _apiClient.GetAsync<TableDto>($"api/table/{tableId}", null);
            return response.Data;
        }

        public async Task<ApiResponse<TableDto>> CreateTableAsync(CreateTableRequestDto request)
        {
            var response = await _apiClient.PostAsync<TableDto>("api/admin/table", request);
            return response;
        }

        public async Task<ApiResponse<TableDto>> UpdateTableAsync(int tableId, UpdateTableRequestDto request)
        {
            var response = await _apiClient.PutAsync<TableDto>($"api/admin/table/{tableId}", request);
            return response;
        }

        public async Task<ApiResponse<object>> DeleteTableAsync(int tableId)
        {
            var response = await _apiClient.DeleteAsync<object>($"api/admin/table/{tableId}", null);
            return response;
        }

        public async Task<IEnumerable<TableDto>> GetTablesByCashierAsync(int zoneId, string type)
        {
            string isOccupied = type == "All" ? "false" : "true";

            var response = await _apiClient.GetCollectionAsync<IEnumerable<TableDto>>($"api/cashier/zones/{zoneId}/table?isOccupied={isOccupied}", null);

            return response.Data;
        }
    }
}
