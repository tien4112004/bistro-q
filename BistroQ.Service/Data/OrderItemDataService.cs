using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Orders;
using BistroQ.Domain.Enums;
using BistroQ.Domain.Models.Entities;
using System.Diagnostics;

namespace BistroQ.Service.Data;

public class OrderItemDataService : IOrderItemDataService
{
    private readonly IApiClient _apiClient;
    private readonly IMapper _mapper;

    public OrderItemDataService(IApiClient apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderItem>> BulkUpdateOrderItemsStatusAsync(IEnumerable<int> orderItemIds, OrderItemStatus status)
    {
        var response = await _apiClient.PutAsync<IEnumerable<DetailOrderItemResponse>>("api/Kitchen/Order/Status",
            new UpdateOrderItemStatusRequest
            {
                OrderItemIds = orderItemIds,
                Status = status
            });

        if (response.Success)
        {
            return _mapper.Map<IEnumerable<OrderItem>>(response.Data);
        }

        throw new Exception("Failed to update order items status");
    }

    public async Task<ApiCollectionResponse<IEnumerable<OrderItem>>> GetOrderItemsAsync(OrderItemColletionQueryParams queryParams = null)
    {
        if (queryParams == null)
        {
            queryParams = new OrderItemColletionQueryParams();
        }

        var response = await _apiClient.GetCollectionAsync<IEnumerable<DetailOrderItemResponse>>("api/Kitchen/Order", queryParams);

        if (response.Success)
        {
            var orderItems = _mapper.Map<IEnumerable<OrderItem>>(response.Data);

            return new ApiCollectionResponse<IEnumerable<OrderItem>>(orderItems, response.TotalItems, response.CurrentPage, response.TotalPages);
        }

        throw new Exception("Failed to get order items");
    }

    public async Task<IEnumerable<OrderItem>> GetOrderItemsByStatusAsync(OrderItemStatus status)
    {
        var response = await _apiClient.GetAsync<IEnumerable<DetailOrderItemResponse>>("api/Kitchen/Order",
            new OrderItemColletionQueryParams
            {
                Status = status.ToString(),
                OrderBy = "CreatedAt",
                OrderDirection = "desc",
                Size = 10000,
            });

        if (response.Success)
        {
            Debug.WriteLine(response.Data.FirstOrDefault().Product.ImageUrl);
            return _mapper.Map<IEnumerable<OrderItem>>(response.Data);
        }

        throw new Exception("Failed to get order items by status");
    }

    public async Task<OrderItem?> UpdateOrderItemStatusAsync(int orderItemId, OrderItemStatus status)
    {
        var response = await _apiClient.PutAsync<IEnumerable<DetailOrderItemResponse>>("api/Kitchen/Order/Status",
            new UpdateOrderItemStatusRequest
            {
                OrderItemIds = new List<int>(orderItemId),
                Status = status
            });

        if (response.Success)
        {
            var orderItems = _mapper.Map<IEnumerable<OrderItem>>(response.Data);
            return orderItems.FirstOrDefault();
        }

        throw new Exception("Failed to update order items status");
    }
}
