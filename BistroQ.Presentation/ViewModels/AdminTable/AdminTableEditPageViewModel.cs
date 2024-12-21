using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Tables;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.AdminTable;

public partial class AdminTableEditPageViewModel : ObservableRecipient, INavigationAware
{
    public ObservableCollection<ZoneViewModel> Zones;
    public TableViewModel Table { get; set; }
    [ObservableProperty]
    private UpdateTableRequest _request;

    [ObservableProperty]
    private bool _isProcessing = false;

    private readonly ITableDataService _tableDataService;
    private readonly IZoneDataService _zoneDataService;
    private readonly IDialogService _dialogService;
    private readonly IMapper _mapper;
    public ICommand EditCommand { get; }

    public AdminTableEditPageViewModel(ITableDataService tableDataService,
        IZoneDataService zoneDataService,
        IDialogService dialogService,
        IMapper mapper)
    {
        _tableDataService = tableDataService;
        _zoneDataService = zoneDataService;
        _dialogService = dialogService;
        _mapper = mapper;
        Request = new UpdateTableRequest();
        Zones = new ObservableCollection<ZoneViewModel>();

        EditCommand = new AsyncRelayCommand(UpdateTableAsync, CanEditTable);
    }

    public void NavigateBack()
    {
        App.GetService<INavigationService>().GoBack();
    }

    public async Task UpdateTableAsync()
    {
        try
        {
            IsProcessing = true;
            if (Request.ZoneId == null)
            {
                throw new InvalidDataException("Please choose a zone.");
            }

            if (Request.SeatsCount == null)
            {
                throw new InvalidDataException("Seats count must be greater than 0.");
            }

            await _tableDataService.UpdateTableAsync(Table.TableId.Value, Request);

            await _dialogService.ShowSuccessDialog("Table updated successfully.", "Success");
            NavigateBack();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorDialog(ex.Message, "Error");
        }
        finally
        {
            IsProcessing = false;
        }
    }

    private bool CanEditTable()
    {
        return !IsProcessing;
    }

    public async Task OnNavigatedTo(object parameter)
    {
        await LoadZonesAsync();

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

    public Task OnNavigatedFrom()
    {
        return Task.CompletedTask;
    }
}