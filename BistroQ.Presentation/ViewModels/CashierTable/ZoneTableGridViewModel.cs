﻿using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BistroQ.Presentation.ViewModels.CashierTable;

public partial class ZoneTableGridViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<TableViewModel> _tables;

    [ObservableProperty]
    private TableViewModel _selectedTableResponse;


    private readonly ITableDataService _tableDataService;
    private readonly IMapper _mapper;

    public ZoneTableGridViewModel(ITableDataService tableDataService, IMapper mapper)
    {
        _tableDataService = tableDataService;
        _mapper = mapper;
    }

    [ObservableProperty]
    private bool _isLoading = true;

    public bool HasTables => Tables != null && Tables.Count > 0;

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
                SelectedTableResponse = Tables.First();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Tables = new ObservableCollection<TableViewModel>();
            SelectedTableResponse = null;
        }
        finally
        {
            IsLoading = false;
        }
    }
}