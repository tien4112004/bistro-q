using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Domain.Services
{
    public class ZoneDataService : IZoneDataService
    {
        private readonly IApiClient _apiClient;

        public ZoneDataService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<PaginationResponse<IEnumerable<Zone>>> GetGridDataAsync(ZoneCollectionQueryParams query = null)
        {
            var response = await _apiClient.GetCollectionAsync<IEnumerable<ZoneResponse>>("/api/admin/zone", query);
            return response;
        }

        public async Task<ApiResponse<Zone>> CreateZoneAsync(CreateZoneRequest request)
        {
            var response = await _apiClient.PostAsync<ZoneResponse>("api/admin/zone", request);
            return response;
        }

        public async Task<ApiResponse<Zone>> UpdateZoneAsync(int zoneId, UpdateZoneRequest request)
        {
            var response = await _apiClient.PutAsync<ZoneResponse>($"api/admin/zone/{zoneId}", request);
            return response;
        }

        public async Task<ApiResponse<object>> DeleteZoneAsync(int zoneId)
        {
            var response = await _apiClient.DeleteAsync<object>($"api/admin/zone/{zoneId}", null);
            return response;
        }

        public async Task<PaginationResponse<IEnumerable<Zone>>> GetZonesAsync(ZoneCollectionQueryParams query = null)
        {
            var response = await _apiClient.GetCollectionAsync<IEnumerable<ZoneResponse>>("/api/zone", query);
            return response;
        }

        public async Task<Zone> GetZoneByIdAsync(int zoneId)
        {
            var response = await _apiClient.GetAsync<ZoneResponse>($"api/zone/{zoneId}", null);
            return response.Data;
        }
    }
}