using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Tables;
using BistroQ.Presentation.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.CashierTable;

public partial class ZoneTableGridViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<TableResponse> _tables;

    [ObservableProperty]
    private TableResponse _selectedTableResponse;


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

        Tables = new ObservableCollection<TableResponse>(tables);
        if (Tables.Any())
        {
            SelectedTableResponse = Tables.First();
        }

        IsLoading = false;
    }
}
