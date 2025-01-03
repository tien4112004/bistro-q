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
    [NotifyPropertyChangedFor(nameof(IsSpaceOccupied))]
    private bool _isOccupied = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsSpaceOccupied))]
    private bool _isCheckingOut = false;

    public bool IsSpaceOccupied => IsOccupied && !IsCheckingOut;

    public TableViewModel Clone()
    {
        return new TableViewModel
        {
            TableId = TableId,
            Name = Name,
            Number = Number,
            ZoneId = ZoneId,
            ZoneName = ZoneName,
            SeatsCount = SeatsCount,
            IsOccupied = IsOccupied,
            IsCheckingOut = IsCheckingOut
        };
    }
}