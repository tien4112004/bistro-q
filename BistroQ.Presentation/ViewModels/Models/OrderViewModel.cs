using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

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
}