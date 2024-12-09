using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Orders;
using BistroQ.Domain.Enums;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Domain.Contracts.Services;

public interface IOrderItemDataService
{
    Task<OrderItem> UpdateOrderItemStatusAsync(int orderItemId, OrderItemStatus status);

    Task<IEnumerable<OrderItem>> BulkUpdateOrderItemsStatusAsync(IEnumerable<int> orderItemIds, OrderItemStatus status);

    Task<IEnumerable<OrderItem>> GetOrderItemsByStatusAsync(OrderItemStatus status);

    Task<ApiCollectionResponse<IEnumerable<OrderItem>>> GetOrderItemsAsync(OrderItemColletionQueryParams queryParams);
}