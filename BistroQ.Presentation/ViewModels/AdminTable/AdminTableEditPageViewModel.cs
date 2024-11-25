using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Tables;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.Contracts.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.AdminTable;

public partial class AdminTableEditPageViewModel : ObservableRecipient, INavigationAware
{
    public TableDto Table { get; set; }
    [ObservableProperty]
    private UpdateTableRequestDto _request;
    public AdminTableEditPageViewModel ViewModel;
    public ObservableCollection<ZoneDto> Zones;

    private readonly ITableDataService _tableDataService;
    private readonly IZoneDataService _zoneDataService;

    public AdminTableEditPageViewModel(ITableDataService tableDataService, IZoneDataService zoneDataService)
    {
        _tableDataService = tableDataService;
        _zoneDataService = zoneDataService;
        Request = new UpdateTableRequestDto();
        Zones = new ObservableCollection<ZoneDto>();

        LoadZonesAsync().ConfigureAwait(false);
    }

    public AdminTableEditPageViewModel()
    {
    }

    public async Task<ApiResponse<TableDto>> UpdateTableAsync()
    {
        if (Request.ZoneId == null)
        {
            throw new InvalidDataException("Please choose a zone.");
        }

        if (Request.SeatsCount == null)
        {
            throw new InvalidDataException("Seats count must be greater than 0.");
        }

        var result = await _tableDataService.UpdateTableAsync(Table.TableId.Value, Request);
        return result;
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is TableDto selectedTable)
        {
            Table = selectedTable;
            Request.SeatsCount = Table?.SeatsCount ?? null;
            Request.ZoneId = Table?.ZoneId ?? null;
        }
    }

    public async Task LoadZonesAsync()
    {
        var response = await _zoneDataService.GetGridDataAsync(new ZoneCollectionQueryParams
        {
            Size = 1000
        });
        Zones.Clear();
        var zones = response.Data;
        foreach (var zone in zones)
        {
            Zones.Add(zone);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
