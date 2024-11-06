﻿using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos;
using BistroQ.Core.Dtos.Tables;

namespace BistroQ.Core.Services
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
    }
}
