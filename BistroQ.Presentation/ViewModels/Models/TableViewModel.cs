using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

public partial class TableViewModel : ObservableObject
{
    [ObservableProperty] 
    private string? _tableId;

    [ObservableProperty] 
    private string? _name;

    [ObservableProperty] 
    private string? _zoneId;
    
    [ObservableProperty]
    private int? _seatsCount;
    
    [ObservableProperty]
    private bool? _isOccupied;
}