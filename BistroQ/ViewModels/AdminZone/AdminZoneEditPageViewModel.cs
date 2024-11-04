using BistroQ.Contracts.ViewModels;
using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos;
using BistroQ.Core.Dtos.Zones;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.ViewModels.AdminZone;

public partial class AdminZoneEditPageViewModel : ObservableRecipient, INavigationAware
{
    public ZoneDto Zone { get; set; }
    [ObservableProperty]
    private UpdateZoneRequestDto request;
    public AdminZoneEditPageViewModel ViewModel;

    private readonly IZoneDataService _zoneDataService;

    public AdminZoneEditPageViewModel(IZoneDataService zoneDataService)
    {
        _zoneDataService = zoneDataService ?? throw new ArgumentNullException(nameof(zoneDataService));
        Request = new UpdateZoneRequestDto();
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
