using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Orders;
using BistroQ.Domain.Enums;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Domain.Contracts.Services;

/// <summary>
/// Interface for managing order item operations through API endpoints.
/// Provides methods for updating and retrieving kitchen order items.
/// </summary>
public interface IOrderItemDataService
{
    /// <summary>
    /// Updates the status of a single order item.
    /// </summary>
    /// <param name="orderItemId">The unique identifier of the order item</param>
    /// <param name="status">The new status to set</param>
    /// <returns>The updated order item if found, null otherwise</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task<OrderItem> UpdateOrderItemStatusAsync(int orderItemId, OrderItemStatus status);

    /// <summary>
    /// Updates the status of multiple order items in a single request.
    /// </summary>
    /// <param name="orderItemIds">Collection of order item identifiers to update</param>
    /// <param name="status">The new status to set for all specified items</param>
    /// <returns>Collection of updated order items</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task<IEnumerable<OrderItem>> BulkUpdateOrderItemsStatusAsync(IEnumerable<string> orderItemIds, OrderItemStatus status);

    /// <summary>
    /// Retrieves all order items with a specific status.
    /// </summary>
    /// <param name="status">The status to filter by</param>
    /// <returns>Collection of order items matching the specified status</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task<IEnumerable<OrderItem>> GetOrderItemsByStatusAsync(OrderItemStatus status);

    /// <summary>
    /// Retrieves a paginated collection of order items based on query parameters.
    /// </summary>
    /// <param name="queryParams">Query parameters for filtering, sorting, and pagination</param>
    /// <returns>A collection response containing order items and pagination information</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task<ApiCollectionResponse<IEnumerable<OrderItem>>> GetOrderItemsAsync(OrderItemColletionQueryParams queryParams);
}