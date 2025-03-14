﻿


using BistroQ.Domain.Enums;

namespace BistroQ.Domain.Models.Entities;

public class Order
{
    public string OrderId { get; set; } = null!;

    public decimal? TotalAmount { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public OrderStatus Status { get; set; }

    public int PeopleCount { get; set; }

    public int? TableId { get; set; }

    public Table? Table { get; set; } = null;

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public decimal? TotalCalories { get; set; }
    public decimal? TotalProtein { get; set; }
    public decimal? TotalFat { get; set; }
    public decimal? TotalFiber { get; set; }
    public decimal? TotalCarbohydrates { get; set; }
}