using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos.Tables;
using BistroQ.Core.Dtos.Zones;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Isolation;

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
            IsLoading = false;
            return;
        }

        var tables = await Task.Run(async () =>
        {
            await Task.Delay(1000);

            return await _tableDataService.GetTablesByCashierAsync((int)zoneId, type);
        });

        Tables = new ObservableCollection<TableDto>(tables);
        if (Tables.Any())
        {
            SelectedTable = Tables.First();
        }

        IsLoading = false;
    }
}