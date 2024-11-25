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

        public async Task<PaginationResponse<IEnumerable<TableResponse>>> GetGridDataAsync(TableCollectionQueryParams query = null)
        {
            var response = await _apiClient.GetCollectionAsync<IEnumerable<TableResponse>>("/api/admin/table", query);
            return response;
        }

        public async Task<TableResponse> GetDataTableAsync(int tableId)
        {
            var response = await _apiClient.GetAsync<TableResponse>($"api/table/{tableId}", null);
            return response.Data;
        }

        public async Task<ApiResponse<TableResponse>> CreateTableAsync(CreateTableRequest request)
        {
            var response = await _apiClient.PostAsync<TableResponse>("api/admin/table", request);
            return response;
        }

        public async Task<ApiResponse<TableResponse>> UpdateTableAsync(int tableId, UpdateTableRequest request)
        {
            var response = await _apiClient.PutAsync<TableResponse>($"api/admin/table/{tableId}", request);
            return response;
        }

        public async Task<ApiResponse<object>> DeleteTableAsync(int tableId)
        {
            var response = await _apiClient.DeleteAsync<object>($"api/admin/table/{tableId}", null);
            return response;
        }

        public async Task<IEnumerable<TableResponse>> GetTablesByCashierAsync(int zoneId, string type)
        {
            string isOccupied = type == "All" ? "false" : "true";

            var response = await _apiClient.GetCollectionAsync<IEnumerable<TableResponse>>($"api/cashier/zones/{zoneId}/table?isOccupied={isOccupied}", null);

            return response.Data;
        }
    }
}
