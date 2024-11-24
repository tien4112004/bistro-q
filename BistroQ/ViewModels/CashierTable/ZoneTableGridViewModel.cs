using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos.Tables;
using BistroQ.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.ViewModels.CashierTable;

public partial class ZoneTableGridViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<TableDto> _tables;

    [ObservableProperty]
    private TableDto _selectedTable;


    private readonly ITableDataService _tableDataService;

    public ZoneTableGridViewModel(ITableDataService tableDataService)
    {
        _tableDataService = tableDataService;
    }

    [ObservableProperty]
    private bool _isLoading = true;

    public bool HasTables => Tables != null && Tables.Count > 0;

    public async Task OnZoneChangedAsync(int? zoneId, string type)
    {
        IsLoading = true;
        if (zoneId == null)
        {
            await Task.Delay(200);
            IsLoading = false;
            return;
        }

        var tables = await TaskHelper.WithMinimumDelay(
            _tableDataService.GetTablesByCashierAsync(zoneId.Value, type),
            200);

        Tables = new ObservableCollection<TableDto>(tables);
        if (Tables.Any())
        {
            SelectedTable = Tables.First();
        }

        IsLoading = false;
    }
}