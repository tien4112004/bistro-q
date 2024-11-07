using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos;
using BistroQ.Core.Dtos.Zones;

namespace BistroQ.Core.Services
{
    public class ZoneDataService : IZoneDataService
    {
        private readonly IApiClient _apiClient;

        public ZoneDataService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<PaginationResponseDto<IEnumerable<ZoneDto>>> GetGridDataAsync(ZoneCollectionQueryParams query = null)
        {
            var response = await _apiClient.GetCollectionAsync<IEnumerable<ZoneDto>>("/api/admin/zone", query);
            return response;
        }

        public async Task<ApiResponse<ZoneDto>> CreateZoneAsync(CreateZoneRequestDto request)
        {
            var response = await _apiClient.PostAsync<ZoneDto>("api/admin/zone", request);
            return response;
        }

        public async Task<ApiResponse<ZoneDto>> UpdateZoneAsync(int zoneId, UpdateZoneRequestDto request)
        {
            var response = await _apiClient.PutAsync<ZoneDto>($"api/admin/zone/{zoneId}", request);
            return response;
        }

        public async Task<ApiResponse<object>> DeleteZoneAsync(int zoneId)
        {
            var response = await _apiClient.DeleteAsync<object>($"api/admin/zone/{zoneId}", null);
            return response;
        }

        public async Task<IEnumerable<ZoneDetailDto>> GetZonesAsync(ZoneCollectionQueryParams query = null)
        {
            var response = await _apiClient.GetCollectionAsync<IEnumerable<ZoneDetailDto>>("/api/zone", query);

            if (response.Success)
            {
                return response.Data;
            }
            else
            {
                return new List<ZoneDetailDto>();
            }
        }

        public async Task<ZoneDetailDto> GetZoneByIdAsync(int zoneId)
        {
            var response = await _apiClient.GetAsync<ZoneDetailDto>($"api/zone/{zoneId}", null);

            if (response.Success)
            {
                return response.Data;
            }
            else
            {
                return new ZoneDetailDto();
            }
        }
    }
}
