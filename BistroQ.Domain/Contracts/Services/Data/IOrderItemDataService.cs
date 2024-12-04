using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Orders;
using BistroQ.Domain.Enums;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Domain.Contracts.Services;

public interface IOrderItemDataService
{
    public Task<OrderItem> UpdateOrderItemStatusAsync(int orderItemId, OrderItemStatus status);

    public Task<IEnumerable<OrderItem>> GetOrderItemsByStatusAsync(OrderItemStatus status);

    public Task<ApiCollectionResponse<IEnumerable<OrderItem>>> GetOrderItemsAsync(OrderItemColletionQueryParams queryParams);
}