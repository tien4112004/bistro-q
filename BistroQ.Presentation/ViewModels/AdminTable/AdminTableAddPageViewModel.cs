using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Tables;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.AdminTable;

public partial class AdminTableAddPageViewModel : ObservableRecipient
{
    [ObservableProperty]
    private CreateTableRequest request;
    public ObservableCollection<ZoneViewModel> Zones;

    private readonly ITableDataService _tableDataService;
    private readonly IZoneDataService _zoneDataService;
    private readonly IMapper _mapper;

    public AdminTableAddPageViewModel(ITableDataService tableDataService, IZoneDataService zoneDataService, IMapper mapper)
    {
        Request = new CreateTableRequest();
        Zones = new ObservableCollection<ZoneViewModel>();
        _tableDataService = tableDataService;
        _zoneDataService = zoneDataService;
        _mapper = mapper;
    }

    public async Task<TableViewModel> AddTable()
    {
        var allZoneList = await _zoneDataService.GetGridDataAsync(new ZoneCollectionQueryParams
        {
            Size = (int)short.MaxValue
        });

        if (Request.ZoneId == null)
        {
            throw new InvalidDataException("Please choose a zone.");
        }

        if (Request.SeatsCount == null)
        {
            throw new InvalidDataException("Seats count must be greater than 0.");
        }

        var table = await _tableDataService.CreateTableAsync(request);
        return _mapper.Map<TableViewModel>(table);
    }

    public async Task LoadZonesAsync()
    {
        try
        {
            var response = await _zoneDataService.GetGridDataAsync(new ZoneCollectionQueryParams());
            Zones.Clear();
            var zones = response.Data;
            foreach (var zone in zones)
            {
                Zones.Add(_mapper.Map<ZoneViewModel>(zone));
            }
        }
        catch (Exception ex)
        {
            Zones = new ObservableCollection<ZoneViewModel>();
        }
    }
}