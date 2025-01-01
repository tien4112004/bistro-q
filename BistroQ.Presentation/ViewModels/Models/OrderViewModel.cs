using BistroQ.Domain.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.Models;

public partial class OrderViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _orderId;

    [ObservableProperty]
    private decimal? _totalAmount;

    [ObservableProperty]
    private DateTime? _startTime;

    [ObservableProperty]
    private DateTime? _endTime;

    [ObservableProperty]
    private OrderStatus _status;

    [ObservableProperty]
    private int _peopleCount;

    [ObservableProperty]
    private int? _tableId;

    public ObservableCollection<OrderItemViewModel> OrderItems { get; set; } = new();

    [ObservableProperty]
    private double _totalCalories;

    [ObservableProperty]
    private double _totalProtein;

    [ObservableProperty]
    private double _totalFat;

    [ObservableProperty]
    private double _totalFiber;

    [ObservableProperty]
    private double _totalCarbohydrates;

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