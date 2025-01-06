using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Order;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Service.Data;

/// <summary>
/// Implementation of order data service that communicates with the API endpoints.
/// Handles both client and cashier order operations with AutoMapper for data transformation.
/// </summary>
public class OrderDataService : IOrderDataService
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
    /// Initializes a new instance of the OrderDataService.
    /// </summary>
    /// <param name="apiClient">Client for making API requests</param>
    /// <param name="mapper">Mapper for data transformation</param>
    public OrderDataService(IApiClient apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }
    #endregion

    #region Public Methods
    public async Task<Order> CreateOrderAsync()
    {
        var response = await _apiClient.PostAsync<DetailOrderResponse>("api/Client/Order", null);

        if (response.Success)
        {
            var order = _mapper.Map<Order>(response.Data);
            return order;
        }

        throw new Exception(response.Message);
    }

    public async Task<Order?> GetOrderAsync()
    {
        var response = await _apiClient.GetAsync<DetailOrderResponse>("api/Client/Order", null);
        if (response.Success)
        {
            var order = _mapper.Map<Order>(response.Data);
            return order;
        }
        throw new Exception(response.Message);
    }

    public async Task DeleteOrderAsync()
    {
        var response = await _apiClient.DeleteAsync<DetailOrderResponse>("api/Client/Order", null);
        if (!response.Success)
        {
            throw new Exception(response.Message);
        }
    }

    public async Task<Order> GetOrderByCashierAsync(int tableId)
    {
        var response = await _apiClient.GetAsync<DetailOrderResponse>($"api/Cashier/Order/{tableId}", null);
        if (response.Success)
        {
            var order = _mapper.Map<Order>(response.Data);
            return order;
        }

        throw new Exception(response.Message);
    }

    public async Task<IEnumerable<Order>> GetCurrentOrdersByCashierAsync()
    {
        var response = await _apiClient.GetAsync<IEnumerable<DetailOrderResponse>>($"api/Cashier/Order", null);
        if (response.Success)
        {
            var orders = _mapper.Map<IEnumerable<Order>>(response.Data);
            return orders;
        }

        throw new Exception(response.Message);
    }

    public async Task<IEnumerable<OrderItem>> CreateOrderItems(IEnumerable<OrderItem> cart)
    {
        var response = await _apiClient.PostAsync<IEnumerable<OrderItem>>($"api/Client/Order/Items", cart);
        if (response.Success)
        {
            var addedItems = response.Data;
            return addedItems;
        }

        throw new Exception(response.Message);
    }

    public async Task ChangePeopleCountAsync(int peopleCount)
    {

        var request = new
        {
            PeopleCount = peopleCount,
        };
        var response = await _apiClient.PatchAsync<Order>($"api/Client/Order/PeopleCount", request);
        if (response.Success)
        {
            return;
        }

        throw new Exception(response.Message);
    }
    #endregion
}