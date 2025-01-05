using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

/// <summary>
/// ViewModel representing a restaurant zone.
/// </summary>
/// <remarks>
/// Tracks zone information and table management details.
/// </remarks>
public partial class ZoneViewModel : ObservableObject
{
    /// <summary>
    /// Gets or sets the unique identifier for the zone
    /// </summary>
    [ObservableProperty]
    private int? _zoneId;

    /// <summary>
    /// Gets or sets the zone name
    /// </summary>
    [ObservableProperty]
    private string? _name;

    /// <summary>
    /// Gets or sets the number of tables in this zone
    /// </summary>
    [ObservableProperty]
    private int _tableCount = 0;

    /// <summary>
    /// Gets or sets whether any tables in the zone are checking out
    /// </summary>
    [ObservableProperty]
    private bool _hasCheckingOutTables = false;
}