using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos.Zones;
using BistroQ.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.ViewModels.CashierTable;

public partial class ZoneOverviewViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<ZoneDto> _zones;

    [ObservableProperty]
    private ZoneDto _selectedZone;

    private readonly IZoneDataService _zoneDataService;

    [ObservableProperty]
    private bool _isLoading = true;

    public ZoneOverviewViewModel(IZoneDataService zoneDataService)
    {
        _zoneDataService = zoneDataService;
    }

    public async Task InitializeAsync()
    {
        IsLoading = true;

        var zones = await TaskHelper.WithMinimumDelay(
            _zoneDataService.GetZonesAsync(new ZoneCollectionQueryParams()),
            200);

        Zones = new ObservableCollection<ZoneDto>(zones.Data);

        if (Zones.Any())
        {
            SelectedZone = Zones.First();
        }
        IsLoading = false;
    }
}
