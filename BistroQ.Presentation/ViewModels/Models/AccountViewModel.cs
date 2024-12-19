using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

public partial class AccountViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _userId;

    [ObservableProperty]
    private string? _username;

    [ObservableProperty]
    private string? _role;

    [ObservableProperty]
    private int? _tableId;

    [ObservableProperty]
    private int? _tableNumber;

    [ObservableProperty]
    private string? _zoneName;

    [ObservableProperty]
    private int? _zoneId;

    public string TableDisplay => TableId.HasValue
        ? $"{ZoneName} - Table {TableNumber}"
        : "No Table Assigned";

    public bool IsSystemAdmin => Role?.ToLower() == "admin" &&
                               Username?.ToLower() == "admin";
}