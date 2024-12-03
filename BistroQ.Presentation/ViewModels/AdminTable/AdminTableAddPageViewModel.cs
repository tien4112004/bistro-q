using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Tables;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Models;
using BistroQ.Presentation.Services;
using CommunityToolkit.Mvvm.Input;

namespace BistroQ.Presentation.ViewModels.AdminTable;

public partial class AdminTableAddPageViewModel : ObservableRecipient
{
    private CreateTableRequest _request;
    
    [ObservableProperty]
    private bool _isProcessing = false;
    [ObservableProperty]
    private AddTableForm _form = new();
    
    public ObservableCollection<ZoneViewModel> Zones;
    
    public event EventHandler NavigateBack;

    private readonly ITableDataService _tableDataService;
    private readonly IZoneDataService _zoneDataService;
    private readonly IDialogService _dialogService;
    private readonly IMapper _mapper;

    public ICommand AddCommand { get; }

    public AdminTableAddPageViewModel(
        ITableDataService tableDataService, 
        IZoneDataService zoneDataService, 
        IDialogService dialogService,
        IMapper mapper)
    {
        _request = new CreateTableRequest();
        Zones = new ObservableCollection<ZoneViewModel>();
        _tableDataService = tableDataService;
        _zoneDataService = zoneDataService;
        _dialogService = dialogService;
        _mapper = mapper;
        AddCommand = new AsyncRelayCommand(AddTableAsync, CanAddTable);
    }
    
    private bool CanAddTable()
    {
        return !IsProcessing;
    }

    public async Task AddTableAsync()
    {
        try
        {
            IsProcessing = true;
            _request.ZoneId = Form.ZoneId;
            _request.SeatsCount = Form.SeatsCount;

            if (_request.ZoneId == 0)
            {
                throw new InvalidDataException("Zone must be selected.");
            }
            
            if (_request.SeatsCount == null)
            {
                throw new InvalidDataException("Seats count must be greater than 0.");
            }

            await _tableDataService.CreateTableAsync(_request);

            await _dialogService.ShowSuccessDialog("Table added successfully.", "Success");
            NavigateBack?.Invoke(this, EventArgs.Empty);
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

    public async Task LoadZonesAsync()
    {
        try
        {
            var response = await _zoneDataService.GetGridDataAsync(null);
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