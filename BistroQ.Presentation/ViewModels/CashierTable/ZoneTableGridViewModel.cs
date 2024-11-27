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

public partial class ZoneTableGridViewModel : ObservableObject, IRecipient<ZoneSelectedMessage>
{
    [ObservableProperty]
    private ObservableCollection<TableViewModel> _tables;

    [ObservableProperty]
    private TableViewModel _selectedTable;


    private readonly ITableDataService _tableDataService;
    private readonly IMapper _mapper;
    private readonly IMessenger _messenger;

    public ZoneTableGridViewModel(ITableDataService tableDataService, IMapper mapper, IMessenger messenger)
    {
        _tableDataService = tableDataService;
        _mapper = mapper;
        _messenger = messenger;
        messenger.RegisterAll(this);
    }

    [ObservableProperty]
    private bool _isLoading = true;

    public bool HasTables => Tables != null && Tables.Count > 0;

    partial void OnSelectedTableChanged(TableViewModel value)
    {
        if (value != null)
        {
            _messenger.Send(new TableSelectedMessage(value.TableId));
        }
    }

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

    public void Receive(ZoneSelectedMessage message)
    {
        if (message != null)
        {
            OnZoneChangedAsync(message.ZoneId, message.Type);
        }
    }
}