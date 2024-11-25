using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Tables;

namespace BistroQ.Service.Data;

public class TableDataService : ITableDataService
{
    private readonly IApiClient _apiClient;
    private readonly IMapper _mapper;

    public TableDataService(IApiClient apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<IEnumerable<Table>>> GetGridDataAsync(TableCollectionQueryParams query = null)
    {
        var response = await _apiClient.GetCollectionAsync<IEnumerable<TableResponse>>("/api/admin/table", query);
        if (response.Success)
        {
            var tables = _mapper.Map<IEnumerable<Table>>(response.Data);
            return new PaginationResponse<IEnumerable<Table>>
                (tables, response.TotalItems, response.CurrentPage, response.TotalPages);
        }
        
        throw new Exception("Failed to get tables");
    }

    public async Task<Table> GetDataTableAsync(int tableId)
    {
        var response = await _apiClient.GetAsync<TableResponse>($"api/table/{tableId}", null);
        if (response.Success)
        {
            return _mapper.Map<Table>(response.Data);
        }
        
        throw new Exception("Failed to get table");
    }

    public async Task<Table> CreateTableAsync(CreateTableRequest request)
    {
        var response = await _apiClient.PostAsync<TableResponse>("api/admin/table", request);
        if(response.Success)
        {
            return _mapper.Map<Table>(response.Data);
        }
        
        throw new Exception("Failed to create table");
    }

    public async Task<Table> UpdateTableAsync(int tableId, UpdateTableRequest request)
    {
        var response = await _apiClient.PutAsync<TableResponse>($"api/admin/table/{tableId}", request);
        if(response.Success)
        {
            return _mapper.Map<Table>(response.Data);
        }
        
        throw new Exception("Failed to update table");
    }

    public async Task DeleteTableAsync(int tableId)
    {
        var response = await _apiClient.DeleteAsync<object>($"api/admin/table/{tableId}", null);
        if (!response.Success)
        {
            throw new Exception(response.Message);
        }
    }

    public async Task<IEnumerable<Table>> GetTablesByCashierAsync(int zoneId, string type)
    {
        string isOccupied = type == "All" ? "false" : "true";

        var response =
            await _apiClient.GetCollectionAsync<IEnumerable<TableResponse>>(
                $"api/cashier/zones/{zoneId}/table?isOccupied={isOccupied}", null);

        if (response.Success)
        {
            return _mapper.Map<IEnumerable<Table>>(response.Data);
        }

        throw new Exception("Failed to get tables");
    }
}