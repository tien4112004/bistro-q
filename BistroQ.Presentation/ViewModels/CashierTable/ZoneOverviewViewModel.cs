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

public partial class ZoneOverviewViewModel : ObservableObject, IRecipient<ZoneStateChangedMessage>, IDisposable
{
    [ObservableProperty]
    private ObservableCollection<ZoneViewModel> _zones;

    [ObservableProperty]
    private ZoneViewModel _selectedZone;


    private readonly IMessenger _messenger;
    private readonly IZoneDataService _zoneDataService;
    private readonly IMapper _mapper;
    private readonly DispatcherQueue dispatcherQueue;

    [ObservableProperty]
    private bool _isLoading = true;

    public ZoneOverviewViewModel(IZoneDataService zoneDataService, IMapper mapper, IMessenger messenger)
    {
        _zoneDataService = zoneDataService;
        _mapper = mapper;
        _messenger = messenger;
        dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        _messenger.RegisterAll(this);
    }

    /**
     * This method is called when the selected zone is changed.
     * https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/generators/observableproperty
     */
    partial void OnSelectedZoneChanged(ZoneViewModel value)
    {
        if (value != null)
        {
            _messenger.Send(new ZoneSelectedMessage(value.ZoneId, "All"));
        }
    }

    public void UpdateFilter(string filterType)
    {
        if (SelectedZone != null)
        {
            _messenger.Send(new ZoneSelectedMessage(SelectedZone.ZoneId, filterType));
        }
    }

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

    public void Dispose()
    {
        _messenger.UnregisterAll(this);
    }
}