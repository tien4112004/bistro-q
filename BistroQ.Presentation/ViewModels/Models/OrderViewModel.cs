using BistroQ.Domain.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.Models;

/// <summary>
/// ViewModel representing an order.
/// </summary>
/// <remarks>
/// Manages order details, status, timing, and nutritional totals.
/// Provides functionality for creating copies of orders.
/// </remarks>
public partial class OrderViewModel : ObservableObject
{
    /// <summary>
    /// Gets or sets the unique identifier for the order
    /// </summary>
    [ObservableProperty]
    private string? _orderId;

    /// <summary>
    /// Gets or sets the total amount for the order
    /// </summary>
    [ObservableProperty]
    private decimal? _totalAmount;

    /// <summary>
    /// Gets or sets the order start time
    /// </summary>
    [ObservableProperty]
    private DateTime? _startTime;

    /// <summary>
    /// Gets or sets the order completion time
    /// </summary>
    [ObservableProperty]
    private DateTime? _endTime;

    /// <summary>
    /// Gets or sets the current order status
    /// </summary>
    [ObservableProperty]
    private OrderStatus _status;

    /// <summary>
    /// Gets or sets the number of people in the order
    /// </summary>
    [ObservableProperty]
    private int _peopleCount;

    /// <summary>
    /// Gets or sets the associated table ID
    /// </summary>
    [ObservableProperty]
    private int? _tableId;

    /// <summary>
    /// Collection of items in this order
    /// </summary>
    public ObservableCollection<OrderItemViewModel> OrderItems { get; set; } = new();

    /// <summary>
    /// Gets or sets the total calories for all items
    /// </summary>
    [ObservableProperty]
    private decimal _totalCalories;

    /// <summary>
    /// Gets or sets the total protein content
    /// </summary>
    [ObservableProperty]
    private decimal _totalProtein;

    /// <summary>
    /// Gets or sets the total fat content
    /// </summary>
    [ObservableProperty]
    private decimal _totalFat;

    /// <summary>
    /// Gets or sets the total fiber content
    /// </summary>
    [ObservableProperty]
    private decimal _totalFiber;

    /// <summary>
    /// Gets or sets the total carbohydrates content
    /// </summary>
    [ObservableProperty]
    private decimal _totalCarbohydrates;

    /// <summary>
    /// Creates a deep copy of the order
    /// </summary>
    /// <returns>A new OrderViewModel instance with copied data</returns>
    public OrderViewModel Clone()
    {
        return new OrderViewModel
        {
            OrderId = OrderId,
            TotalAmount = TotalAmount,
            StartTime = StartTime,
            EndTime = EndTime,
            Status = Status,
            PeopleCount = PeopleCount,
            TableId = TableId,
            TotalCalories = TotalCalories,
            TotalProtein = TotalProtein,
            TotalFat = TotalFat,
            TotalFiber = TotalFiber,
            TotalCarbohydrates = TotalCarbohydrates,
            OrderItems = new ObservableCollection<OrderItemViewModel>(OrderItems)
        };
    }
}