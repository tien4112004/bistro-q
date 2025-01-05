using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BistroQ.Presentation.ViewModels.CashierTable;

/// <summary>
/// ViewModel for managing the grid display of tables within a zone.
/// Handles table selection, filtering, and zone state updates.
/// </summary>
/// <remarks>
/// Implements multiple interfaces for functionality:
/// - ObservableObject for MVVM pattern
/// - IRecipient for handling zone selection messages
/// - IDisposable for resource cleanup
/// </remarks>
public partial class ZoneTableGridViewModel :
    ObservableObject,
    IRecipient<ZoneSelectedMessage>,
    IDisposable
{
    #region Private Fields
    /// <summary>
    /// Service for managing table data operations.
    /// </summary>
    private readonly ITableDataService _tableDataService;

    /// <summary>
    /// Service for object mapping operations.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Service for handling messaging between components.
    /// </summary>
    private readonly IMessenger _messenger;

    /// <summary>
    /// Collection of tables in the selected zone.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<TableViewModel> _tables;

    /// <summary>
    /// Currently selected table in the grid.
    /// </summary>
    [ObservableProperty]
    private TableViewModel _selectedTable;

    /// <summary>
    /// Flag indicating whether data is being loaded.
    /// </summary>
    [ObservableProperty]
    private bool _isLoading = true;
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets whether there are any tables in the current zone.
    /// </summary>
    public bool HasTables => Tables != null && Tables.Count > 0;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the ZoneTableGridViewModel class.
    /// </summary>
    /// <param name="tableDataService">Service for table data operations.</param>
    /// <param name="mapper">Service for object mapping.</param>
    /// <param name="messenger">Service for messaging between components.</param>
    public ZoneTableGridViewModel(ITableDataService tableDataService, IMapper mapper, IMessenger messenger)
    {
        _tableDataService = tableDataService;
        _mapper = mapper;
        _messenger = messenger;
        messenger.RegisterAll(this);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Updates the tables displayed when the selected zone changes.
    /// </summary>
    /// <param name="zoneId">ID of the selected zone.</param>
    /// <param name="type">Type of tables to display.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task OnZoneChangedAsync(int? zoneId, string type)
    {
        IsLoading = true;
        try
        {
            if (zoneId == null)
            {
                await Task.Delay(200);
                IsLoading = false;
                return;
            }

            var tablesData = await TaskHelper.WithMinimumDelay(
                _tableDataService.GetTablesByCashierAsync(zoneId.Value, type),
                200);

            var tables = _mapper.Map<IEnumerable<TableViewModel>>(tablesData);
            Tables = new ObservableCollection<TableViewModel>(tables);
            if (Tables.Any())
            {
                SelectedTable = Tables.First();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Tables = new ObservableCollection<TableViewModel>();
            SelectedTable = null;
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Updates the zone's state based on the checkout status of its tables.
    /// </summary>
    public void UpdateZoneState()
    {
        var hasCheckingOutTables = Tables.Any(t => t.IsCheckingOut);
        _messenger.Send(new ZoneStateChangedMessage(SelectedTable.ZoneName, hasCheckingOutTables));
    }

    /// <summary>
    /// Handles zone selection messages by updating the displayed tables.
    /// </summary>
    /// <param name="message">Message containing the zone selection information.</param>
    public void Receive(ZoneSelectedMessage message)
    {
        if (message != null)
        {
            OnZoneChangedAsync(message.ZoneId, message.Type);
        }
    }

    /// <summary>
    /// Performs cleanup of resources.
    /// </summary>
    public void Dispose()
    {
        _messenger.UnregisterAll(this);
    }
    #endregion

    #region Property Change Handlers
    /// <summary>
    /// Handles changes to the selected table and notifies subscribers.
    /// </summary>
    /// <param name="value">The newly selected table.</param>
    partial void OnSelectedTableChanged(TableViewModel value)
    {
        if (value != null)
        {
            _messenger.Send(new TableSelectedMessage(value.TableId));
        }
    }
    #endregion
}