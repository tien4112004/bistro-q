using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

public partial class ZoneViewModel : ObservableObject
{
    [ObservableProperty]
    private int? _zoneId;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private int _tableCount = 0;

    [ObservableProperty]
    private bool _hasCheckingOutTables = false;
}