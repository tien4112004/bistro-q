using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Service.Data
{
    public class ZoneDataService : IZoneDataService
    {
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;

        public ZoneDataService(IApiClient apiClient, IMapper mapper)
        {
            _apiClient = apiClient;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<IEnumerable<Zone>>> GetGridDataAsync(ZoneCollectionQueryParams query = null)
        {
            var response = await _apiClient.GetCollectionAsync<IEnumerable<ZoneResponse>>("/api/admin/zone", query);
            if (response.Success)
            {
                var zones = _mapper.Map<IEnumerable<Zone>>(response.Data);
                return new PaginationResponse<IEnumerable<Zone>>
                    (zones, response.TotalItems, response.CurrentPage, response.TotalPages);
            }
            throw new Exception("Failed to get zones");
        }

        public async Task<Zone> CreateZoneAsync(CreateZoneRequest request)
        {
            var response = await _apiClient.PostAsync<ZoneResponse>("api/admin/zone", request);
            if (response.Success)
            {
                return _mapper.Map<Zone>(response.Data);
            }
            
            throw new Exception(response.Message);
        }

        public async Task<Zone> UpdateZoneAsync(int zoneId, UpdateZoneRequest request)
        {
            var response = await _apiClient.PutAsync<ZoneResponse>($"api/admin/zone/{zoneId}", request);
            
            if (response.Success)
            {
                return _mapper.Map<Zone>(response.Data);
            }
            
            throw new Exception(response.Message);
        }

        public async Task<bool> DeleteZoneAsync(int zoneId)
        {
            var response = await _apiClient.DeleteAsync<object>($"api/admin/zone/{zoneId}", null);
            if (!response.Success)
            {
                throw new Exception(response.Message);
            }
            
            return true;
        }

        public async Task<PaginationResponse<IEnumerable<Zone>>> GetZonesAsync(ZoneCollectionQueryParams query = null)
        {
            var response = await _apiClient.GetCollectionAsync<IEnumerable<ZoneResponse>>("/api/zone", query);
            
            if (response.Success)
            {
                var zones = _mapper.Map<IEnumerable<Zone>>(response.Data);
                return new PaginationResponse<IEnumerable<Zone>>
                    (zones, response.TotalItems, response.CurrentPage, response.TotalPages);
            }
            
            throw new Exception("Failed to get zones");
        }

        public async Task<Zone> GetZoneByIdAsync(int zoneId)
        {
            var response = await _apiClient.GetAsync<ZoneResponse>($"api/zone/{zoneId}", null);
            
            if (response.Success)
            {
                return _mapper.Map<Zone>(response.Data);
            }
            
            throw new Exception(response.Message);
        }
    }
}