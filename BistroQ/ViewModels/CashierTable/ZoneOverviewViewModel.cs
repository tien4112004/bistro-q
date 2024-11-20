using BistroQ.Contracts.ViewModels;
using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos.Zones;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BistroQ.ViewModels.CashierTable;

public partial class ZoneOverviewViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<ZoneDto> _zones;

    [ObservableProperty]
    private ZoneDto _selectedZone;

    private readonly IZoneDataService _zoneDataService;

    public ZoneOverviewViewModel(IZoneDataService zoneDataService)
    {
        _zoneDataService = zoneDataService;
    }

    public async Task InitializeAsync()
    {
        var zones = await _zoneDataService.GetZonesAsync(null);
        Zones = new ObservableCollection<ZoneDto>(zones.Data);

        if (Zones.Any())
        {
            SelectedZone = Zones.First();
        }
    }
}
