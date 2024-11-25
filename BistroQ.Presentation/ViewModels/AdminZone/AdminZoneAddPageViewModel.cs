using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.AdminZone;

public partial class AdminZoneAddPageViewModel : ObservableRecipient
{
    [ObservableProperty]
    private CreateZoneRequest request;

    private readonly IZoneDataService _zoneDataService;
    private readonly IMapper _mapper;

    public AdminZoneAddPageViewModel(IZoneDataService zoneDataService, IMapper mapper)
    {
        Request = new CreateZoneRequest();
        _zoneDataService = zoneDataService;
        _mapper = mapper;
    }

    public async Task<ZoneViewModel> AddZone()
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            throw new InvalidDataException("Name cannot be null");
        }
        var zone = await _zoneDataService.CreateZoneAsync(request);
        return _mapper.Map<ZoneViewModel>(zone);
    }

    //public void OnNavigatedFrom()
    //{

    //}

    //public void OnNavigatedTo(object parameter)
    //{
    //    throw new NotImplementedException();
    //}
}