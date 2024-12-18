using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Account;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.AdminAccount;

public partial class AdminAccountEditPageViewModel : ObservableRecipient
{
    public ObservableCollection<ZoneViewModel> Zones;
    public ObservableCollection<TableViewModel> Tables;
    public ObservableCollection<string> Roles;
    public AccountViewModel Account { get; set; }

    [ObservableProperty]
    private UpdateAccountRequest _request;

    [ObservableProperty]
    private bool _isProcessing = false;

    [ObservableProperty]
    private bool _isPasswordEditEnabled = false;

    [ObservableProperty]
    private bool _isTableSelectionEnabled = false;

    [ObservableProperty]
    private int? _selectedZoneId;

    private readonly IAccountDataService _accountDataService;
    private readonly IZoneDataService _zoneDataService;
    private readonly ITableDataService _tableDataService;
    private readonly IDialogService _dialogService;
    private readonly IMapper _mapper;

    public ICommand EditCommand { get; }
    public ICommand EnablePasswordEditCommand { get; }

    public event EventHandler NavigateBack;

    public AdminAccountEditPageViewModel(
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

        Request = new UpdateAccountRequest();
        Zones = new ObservableCollection<ZoneViewModel>();
        Tables = new ObservableCollection<TableViewModel>();
        Roles = new ObservableCollection<string> { "Admin", "Staff", "Customer" };

        EditCommand = new AsyncRelayCommand(UpdateAccountAsync, CanEditAccount);
        EnablePasswordEditCommand = new RelayCommand(EnablePasswordEdit);

        this.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(SelectedZoneId) && SelectedZoneId.HasValue)
            {
                _ = LoadTablesAsync(SelectedZoneId.Value);
            }
        };
    }

    private void EnablePasswordEdit()
    {
        IsPasswordEditEnabled = true;
    }

    private bool CanEditAccount()
    {
        return !IsProcessing;
    }

    public async Task UpdateAccountAsync()
    {
        try
        {
            IsProcessing = true;

            if (string.IsNullOrWhiteSpace(Request.Username))
            {
                throw new InvalidDataException("Username cannot be empty.");
            }

            if (IsPasswordEditEnabled && string.IsNullOrWhiteSpace(Request.Password))
            {
                throw new InvalidDataException("Password cannot be empty when editing.");
            }

            if (string.IsNullOrWhiteSpace(Request.Role))
            {
                throw new InvalidDataException("Role must be selected.");
            }

            await _accountDataService.UpdateAccountAsync(Account.UserId, Request);

            await _dialogService.ShowSuccessDialog("Account updated successfully.", "Success");
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

    public void OnNavigatedTo(AccountViewModel account)
    {
        if (account != null)
        {
            Account = account;
            Request.Username = Account.Username;
            Request.Role = Account.Role;
            Request.TableId = Account.TableId;
            SelectedZoneId = Account.TableId != null ? Tables.FirstOrDefault(t => t.TableId == Account.TableId)?.ZoneId : null;
        }
    }
}