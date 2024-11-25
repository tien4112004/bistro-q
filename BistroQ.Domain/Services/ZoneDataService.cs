using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Zones;

namespace BistroQ.Domain.Services
{
    public class ZoneDataService : IZoneDataService
    {
        private readonly IApiClient _apiClient;

        public ZoneDataService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<PaginationResponse<IEnumerable<ZoneDto>>> GetGridDataAsync(ZoneCollectionQueryParams query = null)
        {
            var response = await _apiClient.GetCollectionAsync<IEnumerable<ZoneDto>>("/api/admin/zone", query);
            return response;
        }

        public async Task<ApiResponse<ZoneDto>> CreateZoneAsync(CreateZoneRequest request)
        {
            var response = await _apiClient.PostAsync<ZoneDto>("api/admin/zone", request);
            return response;
        }

        public async Task<ApiResponse<ZoneDto>> UpdateZoneAsync(int zoneId, UpdateZoneRequest request)
        {
            var response = await _apiClient.PutAsync<ZoneDto>($"api/admin/zone/{zoneId}", request);
            return response;
        }

        public async Task<ApiResponse<object>> DeleteZoneAsync(int zoneId)
        {
            var response = await _apiClient.DeleteAsync<object>($"api/admin/zone/{zoneId}", null);
            return response;
        }

        public async Task<PaginationResponse<IEnumerable<ZoneDto>>> GetZonesAsync(ZoneCollectionQueryParams query = null)
        {
            var response = await _apiClient.GetCollectionAsync<IEnumerable<ZoneDto>>("/api/zone", query);
            return response;
        }

        public async Task<ZoneDto> GetZoneByIdAsync(int zoneId)
        {
            var response = await _apiClient.GetAsync<ZoneDto>($"api/zone/{zoneId}", null);
            return response.Data;
        }
    }
}
