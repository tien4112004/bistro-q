using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

/// <summary>
/// ViewModel representing user account information and associated table assignment.
/// </summary>
/// <remarks>
/// Provides properties for user identification, role management, and table assignment tracking.
/// </remarks>
public partial class AccountViewModel : ObservableObject
{
    /// <summary>
    /// Gets or sets the unique identifier for the user
    /// </summary>
    [ObservableProperty]
    private string? _userId;

    /// <summary>
    /// Gets or sets the username
    /// </summary>
    [ObservableProperty]
    private string? _username;

    /// <summary>
    /// Gets or sets the user's role
    /// </summary>
    [ObservableProperty]
    private string? _role;

    /// <summary>
    /// Gets or sets the assigned table's ID
    /// </summary>
    [ObservableProperty]
    private int? _tableId;

    /// <summary>
    /// Gets or sets the assigned table's number
    /// </summary>
    [ObservableProperty]
    private int? _tableNumber;

    /// <summary>
    /// Gets or sets the zone name where the table is located
    /// </summary>
    [ObservableProperty]
    private string? _zoneName;

    /// <summary>
    /// Gets or sets the zone ID where the table is located
    /// </summary>
    [ObservableProperty]
    private int? _zoneId;

    /// <summary>
    /// Gets a formatted display string for the assigned table
    /// </summary>
    public string TableDisplay => TableId.HasValue
        ? $"{ZoneName} - Table {TableNumber}"
        : "No Table Assigned";

    /// <summary>
    /// Gets whether the account has system administrator privileges
    /// </summary>
    public bool IsSystemAdmin => Role?.ToLower() == "admin" &&
                               Username?.ToLower() == "admin";
}
