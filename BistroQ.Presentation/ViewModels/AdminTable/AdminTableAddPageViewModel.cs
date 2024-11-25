using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Tables;
using BistroQ.Domain.Dtos.Zones;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.AdminTable;

public partial class AdminTableAddPageViewModel : ObservableRecipient
{
    [ObservableProperty]
    private CreateTableRequest request;
    public ObservableCollection<ZoneDto> Zones;

    private readonly ITableDataService _tableDataServic;
    private readonly IZoneDataService _zoneDataService;

    public AdminTableAddPageViewModel(ITableDataService tableDataService, IZoneDataService zoneDataService)
    {
        Request = new CreateTableRequest();
        Zones = new ObservableCollection<ZoneDto>();
        _tableDataServic = tableDataService;
        _zoneDataService = zoneDataService;
    }

    public AdminTableAddPageViewModel()
    {
    }

    public async Task<ApiResponse<TableResponse>> AddTable()
    {
        var allZoneList = await _zoneDataService.GetGridDataAsync(new Domain.Dtos.Zones.ZoneCollectionQueryParams
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

        var result = await _tableDataServic.CreateTableAsync(request);
        return result;
    }

    public async Task LoadZonesAsync()
    {
        var response = await _zoneDataService.GetGridDataAsync(new ZoneCollectionQueryParams());
        Zones.Clear();
        var zones = response.Data;
        foreach (var zone in zones)
        {
            Zones.Add(zone);
        }
    }
}
