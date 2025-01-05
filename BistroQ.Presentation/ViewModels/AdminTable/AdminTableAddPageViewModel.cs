using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Tables;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Models;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.AdminTable;

/// <summary>
/// ViewModel for adding new tables in the admin interface.
/// Manages the table creation form and zone selection.
/// </summary>
/// <remarks>
/// Uses MVVM pattern with ObservableRecipient as its base class for property change notifications
/// and command handling.
/// </remarks>
public partial class AdminTableAddPageViewModel : ObservableRecipient
{
    #region Private Fields
    private readonly ITableDataService _tableDataService;
    private readonly IZoneDataService _zoneDataService;
    private readonly IDialogService _dialogService;
    private readonly IMapper _mapper;
    #endregion

    #region Observable Properties
    /// <summary>
    /// Indicates whether the ViewModel is currently processing a request.
    /// </summary>
    [ObservableProperty]
    private bool _isProcessing = false;

    /// <summary>
    /// The form containing table creation data.
    /// </summary>
    [ObservableProperty]
    private AddTableForm _form = new();
    #endregion

    #region Public Properties
    /// <summary>
    /// Collection of available zones for table assignment.
    /// </summary>
    public ObservableCollection<ZoneViewModel> Zones;
    #endregion

    #region Commands
    /// <summary>
    /// Command for adding a new table.
    /// </summary>
    public ICommand AddCommand { get; }
    #endregion

    #region Constructor
    public AdminTableAddPageViewModel(
        ITableDataService tableDataService,
        IZoneDataService zoneDataService,
        IDialogService dialogService,
        IMapper mapper)
    {
        Zones = new ObservableCollection<ZoneViewModel>();
        _tableDataService = tableDataService;
        _zoneDataService = zoneDataService;
        _dialogService = dialogService;
        _mapper = mapper;
        AddCommand = new AsyncRelayCommand(AddTableAsync, CanAddTable);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Determines whether a table can be added based on the processing state.
    /// </summary>
    /// <returns>True if a table can be added; otherwise, false.</returns>
    private bool CanAddTable()
    {
        return !IsProcessing;
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
    /// Adds a new table using the form data.
    /// Validates the input, creates the table, and handles any errors.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddTableAsync()
    {
        try
        {
            IsProcessing = true;
            var request = new CreateTableRequest
            {
                ZoneId = Form.ZoneId,
                SeatsCount = Form.SeatsCount
            };

            if (request.ZoneId == 0)
            {
                throw new InvalidDataException("Zone must be selected.");
            }
            if (request.SeatsCount == null)
            {
                throw new InvalidDataException("Seats count must be greater than 0.");
            }

            await _tableDataService.CreateTableAsync(request);
            await _dialogService.ShowSuccessDialog("Table added successfully.", "Success");
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
    /// Loads the list of available zones for table assignment.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
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
    #endregion
}