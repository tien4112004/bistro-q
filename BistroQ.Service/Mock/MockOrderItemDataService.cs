using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Service.Mock;

public class MockOrderItemDataService : IOrderItemDataService
{
    public MockOrderItemDataService()
    {

    }

    public Task<OrderItem> UpdateOrderItemStatusAsync(int orderItemId, OrderItemStatus status)
    {
        var orderItem = _orderItems.FirstOrDefault(x => x.OrderItemId == orderItemId);
        if (orderItem != null)
        {
            orderItem.Status = status;
            return Task.FromResult(orderItem);
        }

        throw new Exception("Order item not found");
    }

    public Task<IEnumerable<OrderItem>> GetOrderItemsByStatusAsync(OrderItemStatus status)
    {
        return Task.FromResult(_orderItems.Where(x => x.Status == status));
    }

    private readonly List<OrderItem> _orderItems = new List<OrderItem>
    {
        new OrderItem
        {
            OrderItemId = 1,
            OrderId = "1",
            ProductId = 1,
            Quantity = 1,
            Status = OrderItemStatus.Pending,
            PriceAtPurchase = 100,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Order = new Order
            {
                OrderId = "1",
                TotalAmount = 100,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Status = "Pending",
                PeopleCount = 1,
                Table = new Table
                {
                    TableId = 1,
                    Name = "Table 1",
                    Number = 1,
                    ZoneName = "Zone 1",
                },
                TableId = 1
            },
            Product = new Product
            {
                ProductId = 1,
                Name = "Product 1",
                Price = 100,
                Unit = "pcs",
                ImageUrl = "https://placehold.jp/300x200.png"
            }
        },
        new OrderItem
        {
            OrderItemId = 2,
            OrderId = "2",
            ProductId = 2,
            Quantity = 2,
            Status = OrderItemStatus.Pending,
            PriceAtPurchase = 200,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Order = new Order
            {
                OrderId = "2",
                TotalAmount = 200,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Status = "Pending",
                PeopleCount = 2,
                Table = new Table
                {
                    TableId = 2,
                    Name = "Table 2",
                    Number = 2,
                    ZoneName = "Zone 2",
                },
                TableId = 2
            },
            Product = new Product
            {
                ProductId = 2,
                Name = "Product 2",
                Price = 200,
                Unit = "pcs",
                ImageUrl = "https://placehold.jp/300x200.png"
            }
        },
        new OrderItem
        {
            OrderItemId = 3,
            OrderId = "3",
            ProductId = 3,
            Quantity = 3,
            Status = OrderItemStatus.InProgress,
            PriceAtPurchase = 300,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Order = new Order
            {
                OrderId = "3",
                TotalAmount = 300,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Status = "InProgress",
                PeopleCount = 3,
                Table = new Table
                {
                    TableId = 3,
                    Name = "Table 3",
                    Number = 3,
                    ZoneName = "Zone 3",
                },
                TableId = 3
            },
            Product = new Product
            {
                ProductId = 3,
                Name = "Product 3",
                Price = 300,
                Unit = "pcs",
                ImageUrl = "https://placehold.jp/300x200.png"
            }
        },
        new OrderItem
        {
            OrderItemId = 4,
            OrderId = "4",
            ProductId = 4,
            Quantity = 4,
            Status = OrderItemStatus.InProgress,
            PriceAtPurchase = 400,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Order = new Order
            {
                OrderId = "4",
                TotalAmount = 400,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Status = "InProgress",
                PeopleCount = 4,
                Table = new Table
                {
                    TableId = 4,
                    Name = "Table 4",
                    Number = 4,
                    ZoneName = "Zone 4",
                },
                TableId = 4
            },
            Product = new Product
            {
                ProductId = 4,
                Name = "Product 4",
                Price = 400,
                Unit = "pcs",
                ImageUrl = "https://placehold.jp/300x200.png"
            }
        }
    };
}