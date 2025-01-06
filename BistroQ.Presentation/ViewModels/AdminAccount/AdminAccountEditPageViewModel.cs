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
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.AdminAccount;

/// <summary>
/// View model for the admin account editing page. Handles the logic for modifying existing admin accounts
/// and managing related data such as zones and tables.
/// </summary>
/// <remarks>
/// This class implements INavigationAware for navigation handling and IDisposable for resource cleanup.
/// It uses the MVVM pattern with ObservableRecipient as its base class.
/// </remarks>
public partial class AdminAccountEditPageViewModel : ObservableRecipient, INavigationAware, IDisposable
{
    #region Public Fields
    /// <summary>
    /// Collection of available zones in the system.
    /// </summary>
    public ObservableCollection<ZoneViewModel> Zones;

    /// <summary>
    /// Collection of available tables in the selected zone.
    /// </summary>
    public ObservableCollection<TableViewModel> Tables;

    /// <summary>
    /// Collection of available user roles in the system.
    /// </summary>
    public ObservableCollection<string> Roles;

    /// <summary>
    /// The account being edited.
    /// </summary>
    public AccountViewModel Account { get; set; }
    #endregion

    #region Private Fields
    private bool _isDisposed = false;

    private readonly IAccountDataService _accountDataService;
    private readonly IZoneDataService _zoneDataService;
    private readonly ITableDataService _tableDataService;
    private readonly IDialogService _dialogService;
    private readonly IMapper _mapper;
    #endregion

    #region Observable Properties
    /// <summary>
    /// The form containing account editing data.
    /// </summary>
    [ObservableProperty]
    private AddAccountForm _form = new();

    /// <summary>
    /// Indicates whether the view model is currently processing a request.
    /// </summary>
    [ObservableProperty]
    private bool _isProcessing = false;

    /// <summary>
    /// Indicates whether password editing is enabled.
    /// </summary>
    [ObservableProperty]
    private bool _isPasswordEditEnabled = false;

    /// <summary>
    /// Indicates whether table selection is enabled in the UI.
    /// </summary>
    [ObservableProperty]
    private bool _isTableSelectionEnabled = false;
    #endregion

    #region Commands
    /// <summary>
    /// Command for updating the account details.
    /// </summary>
    public ICommand EditCommand { get; }

    /// <summary>
    /// Command for enabling password editing.
    /// </summary>
    public ICommand EnablePasswordEditCommand { get; }
    #endregion

    #region Constructor
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

        this.PropertyChanged += OnPropertyChanged;
    }
    #endregion

    #region Event Handlers
    /// <summary>
    /// Handles property changes in the view model.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments containing the property name.</param>
    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Form.ZoneId) && Form.ZoneId.HasValue)
        {
            _ = LoadTablesAsync(Form.ZoneId.Value);
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Enables password editing functionality.
    /// </summary>
    private void EnablePasswordEdit()
    {
        IsPasswordEditEnabled = true;
    }

    /// <summary>
    /// Determines whether the account can be edited based on current state.
    /// </summary>
    /// <returns>True if the account can be edited; otherwise, false.</returns>
    private bool CanEditAccount()
    {
        return !IsProcessing && !Form.HasErrors;
    }

    /// <summary>
    /// Validates the form fields before submission.
    /// </summary>
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
    #endregion

    #region Public Methods
    /// <summary>
    /// Navigates back to the previous page.
    /// </summary>
    public void NavigateBack()
    {
        App.GetService<INavigationService>().GoBack();
    }

    /// <summary>
    /// Updates the account with the provided form data.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
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
            NavigateBack();
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

    /// <summary>
    /// Loads the list of available zones.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Loads the tables for a specific zone.
    /// </summary>
    /// <param name="zoneId">The ID of the zone to load tables for.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Handles navigation to this page.
    /// </summary>
    /// <param name="parameter">Navigation parameter containing the account to edit.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task OnNavigatedTo(object parameter)
    {
        if (parameter is AccountViewModel account)
        {
            Account = account;
            await LoadZonesAsync();

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

    /// <summary>
    /// Handles navigation from this page.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task OnNavigatedFrom()
    {
        Dispose();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;
        this.PropertyChanged -= OnPropertyChanged;
    }
    #endregion
}