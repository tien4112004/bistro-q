﻿using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Service.Data;

/// <summary>
/// Implementation of zone data service that communicates with the API endpoints.
/// Handles admin, cashier, and general access operations with AutoMapper for data transformation.
/// </summary>
public class ZoneDataService : IZoneDataService
{
    #region Private Fields
    /// <summary>
    /// Client for making HTTP requests to the API.
    /// </summary>
    private readonly IApiClient _apiClient;

    /// <summary>
    /// Mapper for converting between DTOs and domain models.
    /// </summary>
    private readonly IMapper _mapper;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the ZoneDataService.
    /// </summary>
    /// <param name="apiClient">Client for making API requests</param>
    /// <param name="mapper">Mapper for data transformation</param>
    public ZoneDataService(IApiClient apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }
    #endregion

    #region Public Methods
    public async Task<ApiCollectionResponse<IEnumerable<ZoneResponse>>> GetGridDataAsync(ZoneCollectionQueryParams query = null)
    {
        var response = await _apiClient.GetCollectionAsync<IEnumerable<ZoneResponse>>("/api/admin/zone", query);
        if (response.Success)
        {
            var zones = response.Data;
            return new ApiCollectionResponse<IEnumerable<ZoneResponse>>
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

    public async Task<ApiCollectionResponse<IEnumerable<Zone>>> GetZonesAsync(ZoneCollectionQueryParams query = null)
    {
        var response = await _apiClient.GetCollectionAsync<IEnumerable<ZoneResponse>>("/api/zone", query);

        if (response.Success)
        {
            var zones = _mapper.Map<IEnumerable<Zone>>(response.Data);
            return new ApiCollectionResponse<IEnumerable<Zone>>
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

    public async Task<ApiCollectionResponse<IEnumerable<Zone>>> GetZonesByCashierAsync(ZoneCollectionQueryParams query = null)
    {
        var response = await _apiClient.GetCollectionAsync<IEnumerable<ZoneResponse>>("/api/cashier/zones", query);

        if (response.Success)
        {
            var zones = _mapper.Map<IEnumerable<Zone>>(response.Data);
            return new ApiCollectionResponse<IEnumerable<Zone>>
                (zones, response.TotalItems, response.CurrentPage, response.TotalPages);
        }

        throw new Exception("Failed to get zones");
    }
    #endregion
}