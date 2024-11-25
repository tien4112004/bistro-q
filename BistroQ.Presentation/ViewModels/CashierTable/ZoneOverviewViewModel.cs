using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BistroQ.Presentation.ViewModels.CashierTable;

public partial class ZoneOverviewViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<ZoneViewModel> _zones;

    [ObservableProperty]
    private ZoneViewModel _selectedZone;

    private readonly IZoneDataService _zoneDataService;
    private readonly IMapper _mapper;

    [ObservableProperty]
    private bool _isLoading = true;

    public ZoneOverviewViewModel(IZoneDataService zoneDataService, IMapper mapper)
    {
        _zoneDataService = zoneDataService;
        _mapper = mapper;
    }

    public async Task InitializeAsync()
    {
        IsLoading = true;
        try
        {
            var zonesData = await TaskHelper.WithMinimumDelay(
                _zoneDataService.GetZonesAsync(new ZoneCollectionQueryParams()),
                200);

            var zones = _mapper.Map<IEnumerable<ZoneViewModel>>(zonesData.Data);
            Zones = new ObservableCollection<ZoneViewModel>(zones);

            if (Zones.Any())
            {
                SelectedZone = Zones.First();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }
}