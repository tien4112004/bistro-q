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
        var response = await _apiClient.PostAsync<OrderDetailResponse>("api/ClientOrder", null);

        if (response.Success)
        {
            var order = _mapper.Map<Order>(response.Data);
            return order;
        }

        throw new Exception(response.Message);
    }

    public async Task<Order?> GetOrderAsync()
    {
        var response = await _apiClient.GetAsync<OrderDetailResponse>("api/ClientOrder", null);
        if (response.Success)
        {
            var order = _mapper.Map<Order>(response.Data);
            return order;
        }
        throw new Exception(response.Message);
    }

    public async Task DeleteOrderAsync()
    {
        var response = await _apiClient.DeleteAsync<OrderDetailResponse>("api/ClientOrder", null);
        if (!response.Success)
        {
            throw new Exception(response.Message);
        }
    }

    public async Task<Order> GetOrderByCashierAsync(int tableId)
    {
        var response = await _apiClient.GetAsync<OrderDetailResponse>($"api/CashierOrder/{tableId}", null);
        if (response.Success)
        {
            var order = _mapper.Map<Order>(response.Data);
            return order;
        }

        throw new Exception(response.Message);
    }

    public async Task<IEnumerable<Order>> GetCurrentOrdersByCashierAsync()
    {
        var response = await _apiClient.GetAsync<IEnumerable<OrderDetailResponse>>($"api/CashierOrder", null);
        if (response.Success)
        {
            var orders = _mapper.Map<IEnumerable<Order>>(response.Data);
            return orders;
        }

        throw new Exception(response.Message);
    }
}