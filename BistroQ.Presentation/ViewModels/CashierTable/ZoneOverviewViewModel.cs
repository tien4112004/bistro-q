using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BistroQ.Presentation.ViewModels.CashierTable;

/// <summary>
/// ViewModel for managing zone overview in the cashier interface.
/// Handles zone selection, filtering, and state management.
/// </summary>
/// <remarks>
/// Implements multiple interfaces for functionality:
/// - ObservableObject for MVVM pattern
/// - IRecipient for handling zone state messages
/// - IDisposable for resource cleanup
/// </remarks>
public partial class ZoneOverviewViewModel : ObservableObject, IRecipient<ZoneStateChangedMessage>, IDisposable
{
    #region Private Fields
    /// <summary>
    /// Service for handling messaging between components.
    /// </summary>
    private readonly IMessenger _messenger;

    /// <summary>
    /// Service for managing zone data operations.
    /// </summary>
    private readonly IZoneDataService _zoneDataService;

    /// <summary>
    /// Service for object mapping operations.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Queue for dispatching UI updates.
    /// </summary>
    private readonly DispatcherQueue dispatcherQueue;

    /// <summary>
    /// Collection of zones available in the overview.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<ZoneViewModel> _zones;

    /// <summary>
    /// Currently selected zone in the overview.
    /// </summary>
    [ObservableProperty]
    private ZoneViewModel _selectedZone;

    /// <summary>
    /// Flag indicating whether data is being loaded.
    /// </summary>
    [ObservableProperty]
    private bool _isLoading = true;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the ZoneOverviewViewModel class.
    /// </summary>
    /// <param name="zoneDataService">Service for zone data operations.</param>
    /// <param name="mapper">Service for object mapping.</param>
    /// <param name="messenger">Service for messaging between components.</param>
    public ZoneOverviewViewModel(IZoneDataService zoneDataService, IMapper mapper, IMessenger messenger)
    {
        _zoneDataService = zoneDataService;
        _mapper = mapper;
        _messenger = messenger;
        dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        _messenger.RegisterAll(this);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Updates the zone filter based on the specified filter type.
    /// </summary>
    /// <param name="filterType">Type of filter to apply.</param>
    public void UpdateFilter(string filterType)
    {
        if (SelectedZone != null)
        {
            _messenger.Send(new ZoneSelectedMessage(SelectedZone.ZoneId, filterType));
        }
    }

    /// <summary>
    /// Initializes the zone overview by loading zone data.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InitializeAsync()
    {
        IsLoading = true;
        try
        {
            var zonesData = await TaskHelper.WithMinimumDelay(
                _zoneDataService.GetZonesByCashierAsync(new ZoneCollectionQueryParams()),
                200);

            var zones = _mapper.Map<IEnumerable<ZoneViewModel>>(zonesData.Data);
            Zones = new ObservableCollection<ZoneViewModel>(zones);

            if (Zones.Any())
            {
                SelectedZone = Zones.First();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Handles zone state change messages and updates the corresponding zone's status.
    /// </summary>
    /// <param name="message">Message containing the zone state change information.</param>
    public void Receive(ZoneStateChangedMessage message)
    {
        dispatcherQueue.TryEnqueue(() =>
        {
            var zone = Zones.FirstOrDefault(z => z.Name == message.ZoneName);
            if (zone != null)
            {
                zone.HasCheckingOutTables = message.HasCheckingoutTables;
            }
        });
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
    /// Handles changes to the selected zone and notifies subscribers.
    /// For more information, see: https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/generators/observableproperty
    /// </summary>
    /// <param name="value">The newly selected zone.</param>
    partial void OnSelectedZoneChanged(ZoneViewModel value)
    {
        if (value != null)
        {
            _messenger.Send(new ZoneSelectedMessage(value.ZoneId, "All"));
        }
    }
    #endregion
}