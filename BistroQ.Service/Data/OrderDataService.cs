using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Order;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Service.Data;

public class OrderDataService : IOrderDataService
{
    private readonly IApiClient _apiClient;
    private readonly IMapper _mapper;

    public OrderDataService(IApiClient apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }

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
        var response = await _apiClient.GetAsync<DetailOrderResponse>($"api/CashierOrder/{tableId}", null);
        if (response.Success)
        {
            var order = _mapper.Map<Order>(response.Data);
            return order;
        }

        throw new Exception(response.Message);
    }

    public async Task<IEnumerable<Order>> GetCurrentOrdersByCashierAsync()
    {
        var response = await _apiClient.GetAsync<IEnumerable<DetailOrderResponse>>($"api/CashierOrder", null);
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

}