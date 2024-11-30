using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos.Tables;
using BistroQ.Core.Dtos.Zones;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BistroQ.ViewModels.AdminTable;

public partial class AdminTableEditPageViewModel : ObservableRecipient
{
    [ObservableProperty]
    private TableDto _table;
    [ObservableProperty]
    private UpdateTableRequestDto _request;
    [ObservableProperty]
    private bool _isProcessing = false;
    [ObservableProperty]
    private ObservableCollection<ZoneDto> _zones;

    private readonly ITableDataService _tableDataService;
    private readonly IZoneDataService _zoneDataService;

    public ICommand EditCommand { get; }
    //public ICommand FormChangedCommand { get; }

    public event EventHandler<string> ShowSuccessDialog;
    public event EventHandler<string> ShowErrorDialog;
    public event EventHandler NavigateBack;

    public AdminTableEditPageViewModel(ITableDataService tableDataService, IZoneDataService zoneDataService)
    {
        _tableDataService = tableDataService;
        _zoneDataService = zoneDataService;
        Request = new UpdateTableRequestDto();
        Zones = new ObservableCollection<ZoneDto>();

        EditCommand = new AsyncRelayCommand(UpdateTableAsync, CanEditTable);
        LoadZonesAsync().ConfigureAwait(false);
    }

    public AdminTableEditPageViewModel()
    {
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

            var result = await _tableDataService.UpdateTableAsync(Table.TableId.Value, Request);

            if (result.Success)
            {
                ShowSuccessDialog?.Invoke(this, "Table added successfully.");
                NavigateBack?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ShowErrorDialog?.Invoke(this, result.Error);

            }
        }
        catch (Exception ex)
        {
            ShowErrorDialog?.Invoke(this, ex.Message);
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
