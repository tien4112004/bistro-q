using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.AdminZone;

public partial class AdminZoneEditPageViewModel : ObservableRecipient, INavigationAware
{
    public ZoneViewModel Zone { get; set; }
    [ObservableProperty]
    private UpdateZoneRequest request;
    public AdminZoneEditPageViewModel ViewModel;

    private readonly IZoneDataService _zoneDataService;
    private readonly IMapper _mapper;

    public AdminZoneEditPageViewModel(IZoneDataService zoneDataService, IMapper mapper)
    {
        _zoneDataService = zoneDataService;
        Request = new UpdateZoneRequest();
        _mapper = mapper;
    }

    public async Task<ZoneViewModel> UpdateZoneAsync()
    {
        if (string.IsNullOrEmpty(Request.Name))
        {
            throw new InvalidDataException("Name cannot be null");
        }

        var zone = await _zoneDataService.UpdateZoneAsync(Zone.ZoneId.Value, Request);
        return _mapper.Map<ZoneViewModel>(zone);
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is ZoneViewModel selectedZone)
        {
            Zone = selectedZone;
            Request.Name = Zone?.Name ?? string.Empty;
        }
    }

    public void OnNavigatedFrom()
    {
    }
}