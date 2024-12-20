using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Account;
using BistroQ.Domain.Dtos.Tables;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Models;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.AdminAccount;

public partial class AdminAccountEditPageViewModel : ObservableRecipient, INavigationAware
{
    public ObservableCollection<ZoneViewModel> Zones;
    public ObservableCollection<TableViewModel> Tables;
    public ObservableCollection<string> Roles;

    public AccountViewModel Account { get; set; }

    [ObservableProperty]
    private AddAccountForm _form = new();

    [ObservableProperty]
    private bool _isProcessing = false;

    [ObservableProperty]
    private bool _isPasswordEditEnabled = false;

    [ObservableProperty]
    private bool _isTableSelectionEnabled = false;

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

        Zones = new ObservableCollection<ZoneViewModel>();
        Tables = new ObservableCollection<TableViewModel>();
        Roles = new ObservableCollection<string> { "Admin", "Kitchen", "Cashier", "Client" };

        EditCommand = new AsyncRelayCommand(UpdateAccountAsync, CanEditAccount);
        EnablePasswordEditCommand = new RelayCommand(EnablePasswordEdit);

        this.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(Form.ZoneId) && Form.ZoneId.HasValue)
            {
                _ = LoadTablesAsync(Form.ZoneId.Value);
            }
        };
    }

    private void EnablePasswordEdit()
    {
        IsPasswordEditEnabled = true;
    }

    private bool CanEditAccount()
    {
        return !IsProcessing && !Form.HasErrors;
    }

    private void ValidateForm()
    {
        if (Form.Password != null)
        {
            Form.ValidateProperty(nameof(Form.Password), Form.Password);
        }

        Form.ValidateProperty(nameof(Form.Username), Form.Username);
        Form.ValidateProperty(nameof(Form.Role), Form.Role);

        if (Form.TableId != null)
        {
            Form.ValidateProperty(nameof(Form.TableId), Form.TableId);
        }
    }

    public async Task UpdateAccountAsync()
    {
        ValidateForm();
        if (!CanEditAccount())
        {
            await _dialogService.ShowErrorDialog("Data is invalid. Please check again.", "Error");
            return;
        }

        try
        {
            IsProcessing = true;

            var request = new UpdateAccountRequest
            {
                Username = Form.Username,
                Role = Form.Role,
                Password = Form.Password,
                TableId = Form.TableId
            };

            await _accountDataService.UpdateAccountAsync(Account.UserId, request);

            await _dialogService.ShowSuccessDialog("Account updated successfully.", "Success");
            NavigateBack?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.StackTrace);
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
            var tables = await _tableDataService.GetGridDataAsync(new TableCollectionQueryParams
            {
                ZoneId = zoneId,
                Size = 1000
            });
            Tables.Clear();

            var tableViewModels = _mapper.Map<IEnumerable<TableViewModel>>(tables.Data);
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

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is AccountViewModel account)
        {
            Account = account;
            Debug.WriteLine("Befor loading zones...");
            await LoadZonesAsync();
            Debug.WriteLine("After loading zones...");

            if (Account.TableId != null)
            {
                await LoadTablesAsync(account.ZoneId ?? 0);
                Form = new AddAccountForm
                {
                    Username = account.Username,
                    Role = account.Role,
                    TableId = account.TableId,
                    Password = null,
                    ZoneId = account.ZoneId
                };
                IsTableSelectionEnabled = true;
            }
            else
            {
                Form = new AddAccountForm
                {
                    Username = account.Username,
                    Role = account.Role,
                };
                IsTableSelectionEnabled = false;
            }
        }
    }

    public void OnNavigatedFrom()
    {
        this.PropertyChanged -= (s, e) =>
        {
            if (e.PropertyName == nameof(Form.ZoneId) && Form.ZoneId.HasValue)
            {
                _ = LoadTablesAsync(Form.ZoneId.Value);
            }
        };
    }
}