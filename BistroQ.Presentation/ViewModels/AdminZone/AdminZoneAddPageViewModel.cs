using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Zones;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.AdminZone;

public partial class AdminZoneAddPageViewModel : ObservableRecipient
{
    [ObservableProperty]
    private CreateZoneRequestDto request;

    private readonly IZoneDataService _zoneDataService;

    public AdminZoneAddPageViewModel(IZoneDataService zoneDataService)
    {
        Request = new CreateZoneRequestDto();
        _zoneDataService = zoneDataService;
    }

    public AdminZoneAddPageViewModel()
    {
    }

    public async Task<ApiResponse<ZoneDto>> AddZone()
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            throw new InvalidDataException("Name cannot be null");
        }
        var result = await _zoneDataService.CreateZoneAsync(request);
        return result;
    }

    //public void OnNavigatedFrom()
    //{

    //}

    //public void OnNavigatedTo(object parameter)
    //{
    //    throw new NotImplementedException();
    //}
}
