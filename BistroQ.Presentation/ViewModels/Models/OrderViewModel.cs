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
    private string? _status;

    [ObservableProperty]
    private int _peopleCount;

    [ObservableProperty]
    private int? _tableId;

    public ObservableCollection<OrderItemViewModel> OrderItems { get; } = new();

    [ObservableProperty]
    private decimal _totalCalories;

    [ObservableProperty]
    private decimal _totalProtein;

    [ObservableProperty]
    private decimal _totalFat;

    [ObservableProperty]
    private decimal _totalFiber;

    [ObservableProperty]
    private decimal _totalCarbohydrates;
}