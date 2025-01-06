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

namespace BistroQ.Presentation.ViewModels.AdminAccount;

/// <summary>
/// View model for the admin account creation page. Handles the logic for adding new admin accounts
/// and managing related data such as zones and tables.
/// </summary>
/// <remarks>
/// This class implements INavigationAware for navigation handling and IDisposable for cleanup.
/// It uses the MVVM pattern with ObservableRecipient as its base class.
/// </remarks>
public partial class AdminAccountAddPageViewModel : ObservableRecipient, INavigationAware, IDisposable
{
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
    /// Indicates whether the view model is currently processing a request.
    /// </summary>
    [ObservableProperty]
    private bool _isProcessing = false;

    /// <summary>
    /// The form containing account creation data.
    /// </summary>
    [ObservableProperty]
    private AddAccountForm _form = new();

    /// <summary>
    /// Indicates whether table selection is enabled in the UI.
    /// </summary>
    [ObservableProperty]
    private bool _isTableSelectionEnabled = false;

    /// <summary>
    /// The currently selected zone ID.
    /// </summary>
    [ObservableProperty]
    private int? _selectedZoneId;

    /// <summary>
    /// Collection of available zones.
    /// </summary>
    public ObservableCollection<ZoneViewModel> Zones;

    /// <summary>
    /// Collection of available tables.
    /// </summary>
    public ObservableCollection<TableViewModel> Tables;

    /// <summary>
    /// Collection of available user roles.
    /// </summary>
    public ObservableCollection<string> Roles;

    /// <summary>
    /// Command for adding a new account.
    /// </summary>
    public IRelayCommand AddCommand { get; }
    #endregion

    #region Constructor
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
        Roles = new ObservableCollection<string> { "Admin", "Kitchen", "Cashier", "Client" };

        AddCommand = new AsyncRelayCommand(AddAccountAsync);

        this.PropertyChanged += OnPropertyChanged;
    }
    #endregion

    #region Event Handlers
    /// <summary>
    /// Handles changes to the form property.
    /// </summary>
    /// <param name="value">The new form value.</param>
    partial void OnFormChanged(AddAccountForm value)
    {
        AddCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Handles property changes in the view model.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments containing the property name.</param>
    private async void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SelectedZoneId) && SelectedZoneId.HasValue)
        {
            try
            {
                await LoadTablesAsync(SelectedZoneId.Value);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
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
    /// Adds a new account with the provided form data.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddAccountAsync()
    {
        Form.ValidateAll();
        if (Form.HasErrors)
        {
            await _dialogService.ShowErrorDialog("Data is invalid. Please check again.", "Error");
            return;
        }

        try
        {
            IsProcessing = true;

            var request = new CreateAccountRequest
            {
                Username = Form.Username,
                Password = Form.Password,
                Role = Form.Role,
                TableId = Form.TableId
            };

            await _accountDataService.CreateAccountAsync(request);

            await _dialogService.ShowSuccessDialog("Account added successfully.", "Success");
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
    /// <param name="parameter">Navigation parameter.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task OnNavigatedTo(object parameter)
    {
        await LoadZonesAsync();
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