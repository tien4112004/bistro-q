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

/// <summary>
/// ViewModel for editing existing tables in the admin interface.
/// Manages the table editing form and zone selection.
/// </summary>
/// <remarks>
/// Implements INavigationAware for handling navigation events and uses MVVM pattern
/// with ObservableRecipient as its base class.
/// </remarks>
public partial class AdminTableEditPageViewModel : ObservableRecipient, INavigationAware
{
    #region Private Fields
    private readonly ITableDataService _tableDataService;
    private readonly IZoneDataService _zoneDataService;
    private readonly IDialogService _dialogService;
    private readonly IMapper _mapper;
    #endregion

    #region Public Properties
    /// <summary>
    /// Collection of available zones for table reassignment.
    /// </summary>
    public ObservableCollection<ZoneViewModel> Zones;

    /// <summary>
    /// The table being edited.
    /// </summary>
    public TableViewModel Table { get; set; }
    #endregion

    #region Observable Properties
    /// <summary>
    /// The request object containing updated table data.
    /// </summary>
    [ObservableProperty]
    private UpdateTableRequest _request;

    /// <summary>
    /// Indicates whether the ViewModel is currently processing a request.
    /// </summary>
    [ObservableProperty]
    private bool _isProcessing = false;
    #endregion

    #region Commands
    /// <summary>
    /// Command for updating the table details.
    /// </summary>
    public ICommand EditCommand { get; }
    #endregion

    #region Constructor
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
    /// Updates the existing table using the form data.
    /// Validates the input, updates the table, and handles any errors.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Loads the list of available zones for table reassignment.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Handles navigation to this page, initializing data with the selected table.
    /// </summary>
    /// <param name="parameter">Navigation parameter containing the table to edit.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Handles navigation from this page.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task OnNavigatedFrom()
    {
        return Task.CompletedTask;
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Determines whether the table can be edited based on the processing state.
    /// </summary>
    /// <returns>True if the table can be edited; otherwise, false.</returns>
    private bool CanEditTable()
    {
        return !IsProcessing;
    }
    #endregion
}