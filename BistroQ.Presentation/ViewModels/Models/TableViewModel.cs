using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

/// <summary>
/// ViewModel representing a restaurant table.
/// </summary>
/// <remarks>
/// Manages table status, location, and capacity information.
/// Provides functionality for creating copies of table data.
/// </remarks>
public partial class TableViewModel : ObservableObject
{
    /// <summary>
    /// Gets or sets the unique identifier for the table
    /// </summary>
    [ObservableProperty]
    private int? _tableId;

    /// <summary>
    /// Gets or sets the table name
    /// </summary>
    [ObservableProperty]
    private string? _name;

    /// <summary>
    /// Gets or sets the table number
    /// </summary>
    [ObservableProperty]
    private int? _number;

    /// <summary>
    /// Gets or sets the associated zone ID
    /// </summary>
    [ObservableProperty]
    private int? _zoneId;

    /// <summary>
    /// Gets or sets the zone name
    /// </summary>
    [ObservableProperty]
    private string? _zoneName;

    /// <summary>
    /// Gets or sets the number of seats at the table
    /// </summary>
    [ObservableProperty]
    private int? _seatsCount;

    /// <summary>
    /// Gets or sets whether the table is currently occupied
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsSpaceOccupied))]
    private bool _isOccupied = false;

    /// <summary>
    /// Gets or sets whether the table is in the process of checking out
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsSpaceOccupied))]
    private bool _isCheckingOut = false;

    /// <summary>
    /// Gets whether the table space is currently occupied (occupied and not checking out)
    /// </summary>
    public bool IsSpaceOccupied => IsOccupied && !IsCheckingOut;

    /// <summary>
    /// Creates a deep copy of the table data
    /// </summary>
    /// <returns>A new TableViewModel instance with copied data</returns>
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