using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.AdminAccount;

public partial class AdminAccountAddPageViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool _isProcessing = false;

    [ObservableProperty]
    private AddAccountForm _form = new();

    [ObservableProperty]
    private bool _isTableSelectionEnabled = false;

    [ObservableProperty]
    private int? _selectedZoneId;

    public ObservableCollection<ZoneViewModel> Zones;
    public ObservableCollection<TableViewModel> Tables;
    public ObservableCollection<string> Roles;

    public event EventHandler NavigateBack;

    private readonly IAccountDataService _accountDataService;
    private readonly IZoneDataService _zoneDataService;
    private readonly ITableDataService _tableDataService;
    private readonly IDialogService _dialogService;
    private readonly IMapper _mapper;

    public ICommand AddCommand { get; }

    public AdminAccountAddPageViewModel(
        IAccountDataService accountDataService,
        IZoneDataService zoneDataService,
        ITableDataService tableDataService,
        IDialogService dialogService,
        IMapper mapper)
    {
        _accountDataService = accountDataService;
        _zoneDataService = zoneDataService;
        _tableDataService = tableDataService;
        _dialogService = dialogService;
        _mapper = mapper;

        Zones = new ObservableCollection<ZoneViewModel>();
        Tables = new ObservableCollection<TableViewModel>();
        Roles = new ObservableCollection<string> { "Admin", "Staff", "Customer" };

        AddCommand = new AsyncRelayCommand(AddAccountAsync, CanAddAccount);

        this.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(SelectedZoneId) && SelectedZoneId.HasValue)
            {
                _ = LoadTablesAsync(SelectedZoneId.Value);
            }
        };
    }

    private bool CanAddAccount()
    {
        return !IsProcessing;
    }

    public async Task AddAccountAsync()
    {
        try
        {
            IsProcessing = true;
            Form.ValidateAll();

            if (Form.HasErrors)
            {
                throw new InvalidDataException("Please fix all validation errors before proceeding.");
            }

            var request = new CreateAccountRequest
            {
                Username = Form.Username,
                Password = Form.Password,
                Role = Form.Role,
                TableId = Form.TableId
            };

            await _accountDataService.CreateAccountAsync(request);

            await _dialogService.ShowSuccessDialog("Account added successfully.", "Success");
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
        catch (Exception ex)
        {
            await _dialogService.ShowErrorDialog(ex.Message, "Error");
        }
    }

    public async Task LoadTablesAsync(int zoneId)
    {
        try
        {
            var tables = await _tableDataService.GetTablesByCashierAsync(zoneId, "All");
            Tables.Clear();
            var tableViewModels = _mapper.Map<IEnumerable<TableViewModel>>(tables);
            foreach (var table in tableViewModels)
            {
                Tables.Add(table);
            }
            IsTableSelectionEnabled = true;
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorDialog(ex.Message, "Error");
            IsTableSelectionEnabled = false;
        }
    }
}