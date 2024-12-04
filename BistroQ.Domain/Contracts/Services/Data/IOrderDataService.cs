using BistroQ.Domain.Models.Entities;

namespace BistroQ.Domain.Contracts.Services;

public interface IOrderDataService
{
    public Task<Order> CreateOrderAsync();

    public Task<Order?> GetOrderAsync();

    public Task DeleteOrderAsync();

    public Task<Order> GetOrderByCashierAsync(int tableId);

    public Task<IEnumerable<Order>> GetCurrentOrdersByCashierAsync();

}
