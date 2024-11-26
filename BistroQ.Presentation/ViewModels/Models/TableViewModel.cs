using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

public partial class TableViewModel : ObservableObject
{
    [ObservableProperty]
    private int? _tableId;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private int? _number;

    [ObservableProperty]
    private int? _zoneId;

    [ObservableProperty]
    private string? _zoneName;

    [ObservableProperty]
    private int? _seatsCount;

    [ObservableProperty]
    private bool? _isOccupied;
}