using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

public partial class ZoneViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _zoneId;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private ObservableCollection<TableViewModel>? _tables;
}