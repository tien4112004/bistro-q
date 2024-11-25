using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.Contracts.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.AdminZone;

public partial class AdminZoneEditPageViewModel : ObservableRecipient, INavigationAware
{
    public ZoneDto Zone { get; set; }
    [ObservableProperty]
    private UpdateZoneRequest request;
    public AdminZoneEditPageViewModel ViewModel;

    private readonly IZoneDataService _zoneDataService;

    public AdminZoneEditPageViewModel(IZoneDataService zoneDataService)
    {
        _zoneDataService = zoneDataService;
        Request = new UpdateZoneRequest();
    }

    public async Task<ApiResponse<ZoneDto>> UpdateZoneAsync()
    {
        if (string.IsNullOrEmpty(Request.Name))
        {
            throw new InvalidDataException("Name cannot be null");
        }

        var result = await _zoneDataService.UpdateZoneAsync(Zone.ZoneId.Value, Request);
        return result;
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is ZoneDto selectedZone)
        {
            Zone = selectedZone;
            Request.Name = Zone?.Name ?? string.Empty;
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
