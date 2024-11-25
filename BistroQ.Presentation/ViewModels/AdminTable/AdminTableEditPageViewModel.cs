using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Tables;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.Contracts.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using AutoMapper;
using BistroQ.Presentation.ViewModels.Models;

namespace BistroQ.Presentation.ViewModels.AdminTable;

public partial class AdminTableEditPageViewModel : ObservableRecipient, INavigationAware
{
    public TableViewModel Table { get; set; }
    [ObservableProperty]
    private UpdateTableRequest _request;
    public AdminTableEditPageViewModel ViewModel;
    public ObservableCollection<ZoneViewModel> Zones;

    private readonly ITableDataService _tableDataService;
    private readonly IZoneDataService _zoneDataService;
    private readonly IMapper _mapper;

    public AdminTableEditPageViewModel(ITableDataService tableDataService, 
        IZoneDataService zoneDataService, 
        IMapper mapper)
    {
        _tableDataService = tableDataService;
        _zoneDataService = zoneDataService;
        _mapper = mapper;
        Request = new UpdateTableRequest();
        Zones = new ObservableCollection<ZoneViewModel>();

        LoadZonesAsync().ConfigureAwait(false);
    }

    public AdminTableEditPageViewModel()
    {
    }

    public async Task<TableViewModel> UpdateTableAsync()
    {
        if (Request.ZoneId == null)
        {
            throw new InvalidDataException("Please choose a zone.");
        }

        if (Request.SeatsCount == null)
        {
            throw new InvalidDataException("Seats count must be greater than 0.");
        }

        var table = await _tableDataService.UpdateTableAsync(Table.TableId.Value, Request);
        return _mapper.Map<TableViewModel>(table);
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is TableViewModel selectedTable)
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
        var zones = _mapper.Map<IEnumerable<ZoneViewModel>>(response.Data);
        foreach (var zone in zones)
        {
            Zones.Add(zone);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}