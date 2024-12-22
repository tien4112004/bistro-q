using BistroQ.Domain.Models.Entities;

namespace BistroQ.Domain.Contracts.Services;

public interface IOrderDataService
{
    Task<Order> CreateOrderAsync();

    Task<Order?> GetOrderAsync();

    Task ChangePeopleCountAsync(int peopleCount);

    Task DeleteOrderAsync();

    Task<Order> GetOrderByCashierAsync(int tableId);

    Task<IEnumerable<Order>> GetCurrentOrdersByCashierAsync();

    Task<IEnumerable<OrderItem>> CreateOrderItems(IEnumerable<OrderItem> cart);


}
