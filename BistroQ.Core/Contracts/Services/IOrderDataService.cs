using BistroQ.Core.Models.Entities;

namespace BistroQ.Core.Contracts.Services;

public interface IOrderDataService
{
    public Task<Order> CreateOrderAsync();

    public Task<Order?> GetOrderAsync();

    public Task DeleteOrderAsync();

    public Task<Order> GetOrderByCashierAsync(int tableId);

    public Task<IEnumerable<Order>> GetCurrentOrdersByCashierAsync();

}
