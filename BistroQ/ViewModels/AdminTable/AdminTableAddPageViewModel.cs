using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos;
using BistroQ.Core.Dtos.Tables;
using BistroQ.Core.Dtos.Zones;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.ViewModels.AdminTable;

public partial class AdminTableAddPageViewModel : ObservableRecipient
{
    [ObservableProperty]
    private CreateTableRequestDto request;
    public ObservableCollection<ZoneDto> Zones;

    private readonly ITableDataService _tableDataServic;
    private readonly IZoneDataService _zoneDataService;

    public AdminTableAddPageViewModel(ITableDataService tableDataService, IZoneDataService zoneDataService)
    {
        Request = new CreateTableRequestDto();
        Zones = new ObservableCollection<ZoneDto>();
        _tableDataServic = tableDataService;
        _zoneDataService = zoneDataService;
    }

    public AdminTableAddPageViewModel()
    {
    }

    public async Task<ApiResponse<TableDto>> AddTable()
    {
        var allZoneList = await _zoneDataService.GetGridDataAsync(new Core.Dtos.Zones.ZoneCollectionQueryParams
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
