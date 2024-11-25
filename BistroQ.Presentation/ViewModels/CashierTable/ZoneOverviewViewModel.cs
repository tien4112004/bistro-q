using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using BistroQ.Presentation.ViewModels.Models;

namespace BistroQ.Presentation.ViewModels.CashierTable;

public partial class ZoneOverviewViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<ZoneViewModel> _zones;

    [ObservableProperty]
    private ZoneViewModel _selectedZone;

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

        Zones = new ObservableCollection<ZoneViewModel>(zones.Data);

        if (Zones.Any())
        {
            SelectedZone = Zones.First();
        }
        IsLoading = false;
    }
}