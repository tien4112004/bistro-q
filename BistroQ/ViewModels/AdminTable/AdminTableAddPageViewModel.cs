using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos.Tables;
using BistroQ.Core.Dtos.Zones;
using BistroQ.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BistroQ.ViewModels.AdminTable;

public partial class AdminTableAddPageViewModel : ObservableRecipient
{
    [ObservableProperty]
    private CreateTableRequestDto _request;
    [ObservableProperty]
    private bool _isProcessing = false;
    [ObservableProperty]
    private AddTableForm _form = new();
    [ObservableProperty]
    private string _errorMessage = string.Empty;
    public ObservableCollection<ZoneDto> Zones;

    private readonly ITableDataService _tableDataServic;
    private readonly IZoneDataService _zoneDataService;

    public ICommand AddCommand { get; }
    public ICommand FormChangedCommand { get; }

    public event EventHandler<string> ShowSuccessDialog;
    public event EventHandler<string> ShowErrorDialog;
    public event EventHandler NavigateBack;

    public AdminTableAddPageViewModel(ITableDataService tableDataService, IZoneDataService zoneDataService)
    {
        _tableDataServic = tableDataService;
        _zoneDataService = zoneDataService;
        Request = new CreateTableRequestDto();

        Zones = new ObservableCollection<ZoneDto>();

        FormChangedCommand = new RelayCommand<(string Field, string Value)>((param) =>
        {
            Form.ValidateProperty(param.Field, param.Value);
            ((AsyncRelayCommand)AddCommand).NotifyCanExecuteChanged();
        });
        AddCommand = new AsyncRelayCommand(AddTableAsync, CanAddTable);
    }

    public AdminTableAddPageViewModel()
    {
    }

    private bool CanAddTable()
    {
        return !Form.HasErrors && !IsProcessing;
    }

    private async Task AddTableAsync()
    {
        try
        {
            IsProcessing = true;
            Request.ZoneId = Form.ZoneId;
            Request.SeatsCount = Form.SeatsCount;

            if (Request.ZoneId == null)
            {
                throw new InvalidDataException("Please choose a zone.");
            }

            if (Request.SeatsCount == null)
            {
                throw new InvalidDataException("Seats count must be greater than 0.");
            }

            var result = await _tableDataServic.CreateTableAsync(_request);

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

    public async Task LoadZonesAsync()
    {
        var response = await _zoneDataService.GetGridDataAsync(new ZoneCollectionQueryParams
        {
            Size = short.MaxValue
        });
        Zones.Clear();
        var zones = response.Data;
        foreach (var zone in zones)
        {
            Zones.Add(zone);
        }
    }
}
