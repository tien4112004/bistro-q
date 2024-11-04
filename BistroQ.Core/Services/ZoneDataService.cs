using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos;
using BistroQ.Core.Dtos.Zones;

namespace BistroQ.Core.Services
{
    public class ZoneDataService : IZoneDataService
    {
        private readonly HttpClient _httpClient;

        public ZoneDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginationResponseDto<IEnumerable<ZoneDto>>> GetGridDataAsync(ZoneCollectionQueryParams query = null)
        {
            var client = new ApiClient(this._httpClient);

            var response = await client.GetCollectionAsync<IEnumerable<ZoneDto>>("/api/admin/zone", query);
            return response;
        }

        public async Task<ApiResponse<ZoneDto>> CreateZoneAsync(CreateZoneRequestDto request)
        {
            var client = new ApiClient(this._httpClient);

            var response = await client.PostAsync<ZoneDto>("api/admin/zone", request);
            return response;
        }

        public async Task<ApiResponse<ZoneDto>> UpdateZoneAsync(int zoneId, UpdateZoneRequestDto request)
        {
            var client = new ApiClient(this._httpClient);

            var response = await client.PutAsync<ZoneDto>($"api/admin/zone/{zoneId}", request);
            return response;
        }

        public async Task<ApiResponse<object>> DeleteZoneAsync(int zoneId)
        {
            var client = new ApiClient(this._httpClient);

            var response = await client.DeleteAsync<object>($"api/admin/zone/{zoneId}");
            return response;
        }
    }
}
