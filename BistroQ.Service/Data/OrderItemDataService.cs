using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Orders;
using BistroQ.Domain.Enums;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Service.Data;

/// <summary>
/// Implementation of order item data service that communicates with the API endpoints.
/// Handles kitchen-specific operations with AutoMapper for data transformation.
/// </summary>
public class OrderItemDataService : IOrderItemDataService
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
    /// Initializes a new instance of the OrderItemDataService.
    /// </summary>
    /// <param name="apiClient">Client for making API requests</param>
    /// <param name="mapper">Mapper for data transformation</param>
    public OrderItemDataService(IApiClient apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }
    #endregion

    #region Public Methods
    public async Task<IEnumerable<OrderItem>> BulkUpdateOrderItemsStatusAsync(IEnumerable<string> orderItemIds, OrderItemStatus status)
    {
        var response = await _apiClient.PatchAsync<IEnumerable<DetailOrderItemResponse>>("api/Kitchen/Order/Status",
            new UpdateOrderItemStatusRequest
            {
                OrderItemIds = orderItemIds.Select(x => x.ToString()),
                Status = status.ToString()
            });

        if (response.Success)
        {
            return _mapper.Map<IEnumerable<OrderItem>>(response.Data);
        }

        throw new Exception(response.Message);
    }

    public async Task<ApiCollectionResponse<IEnumerable<OrderItem>>> GetOrderItemsAsync(OrderItemColletionQueryParams queryParams = null)
    {
        if (queryParams == null)
        {
            queryParams = new OrderItemColletionQueryParams();
        }
        queryParams.OrderBy = "UpdatedAt";
        queryParams.OrderDirection = "desc";

        var response = await _apiClient.GetCollectionAsync<IEnumerable<DetailOrderItemResponse>>("api/Kitchen/Order", queryParams);

        if (response.Success)
        {
            var orderItems = _mapper.Map<IEnumerable<OrderItem>>(response.Data);

            return new ApiCollectionResponse<IEnumerable<OrderItem>>(orderItems, response.TotalItems, response.CurrentPage, response.TotalPages);
        }

        throw new Exception(response.Message);
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
            return _mapper.Map<IEnumerable<OrderItem>>(response.Data);
        }

        throw new Exception(response.Message);
    }

    public async Task<OrderItem?> UpdateOrderItemStatusAsync(int orderItemId, OrderItemStatus status)
    {
        var response = await _apiClient.PatchAsync<IEnumerable<DetailOrderItemResponse>>("api/Kitchen/Order/Status",
            new UpdateOrderItemStatusRequest
            {
                OrderItemIds = new List<string>(orderItemId),
                Status = status.ToString()
            });

        if (response.Success)
        {
            var orderItems = _mapper.Map<IEnumerable<OrderItem>>(response.Data);
            return orderItems.FirstOrDefault();
        }

        throw new Exception(response.Message);
    }
    #endregion
}
