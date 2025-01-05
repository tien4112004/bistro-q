using BistroQ.Domain.Models.Entities;

namespace BistroQ.Domain.Contracts.Services;

/// <summary>
/// Interface for managing order-related data operations through API endpoints.
/// Provides methods for handling orders from both client and cashier perspectives.
/// </summary>
public interface IOrderDataService
{
    /// <summary>
    /// Creates a new order for the current client session.
    /// </summary>
    /// <returns>The newly created order</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task<Order> CreateOrderAsync();

    /// <summary>
    /// Retrieves the current active order for the client session.
    /// </summary>
    /// <returns>The current order if it exists, null otherwise</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task<Order?> GetOrderAsync();

    /// <summary>
    /// Updates the number of people for the current order.
    /// </summary>
    /// <param name="peopleCount">The new count of people for the order</param>
    /// <returns>A task representing the asynchronous operation</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task ChangePeopleCountAsync(int peopleCount);

    /// <summary>
    /// Deletes the current order for the client session.
    /// </summary>
    /// <returns>A task representing the asynchronous operation</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task DeleteOrderAsync();

    /// <summary>
    /// Retrieves order details for a specific table (Cashier endpoint).
    /// </summary>
    /// <param name="tableId">The unique identifier of the table</param>
    /// <returns>The order details for the specified table</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task<Order> GetOrderByCashierAsync(int tableId);

    /// <summary>
    /// Retrieves all current active orders (Cashier endpoint).
    /// </summary>
    /// <returns>Collection of currently active orders</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task<IEnumerable<Order>> GetCurrentOrdersByCashierAsync();

    /// <summary>
    /// Creates multiple order items from the provided cart.
    /// </summary>
    /// <param name="cart">Collection of order items to be added</param>
    /// <returns>Collection of successfully added order items</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task<IEnumerable<OrderItem>> CreateOrderItems(IEnumerable<OrderItem> cart);
}