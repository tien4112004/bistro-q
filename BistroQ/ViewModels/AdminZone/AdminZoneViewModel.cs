using BistroQ.Contracts.ViewModels;
using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos.Zones;
using BistroQ.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BistroQ.ViewModels;

public partial class AdminZoneViewModel : ObservableRecipient, INavigationAware
{
    private readonly IZoneDataService _zoneDataService;

    [ObservableProperty]
    private ObservableCollection<ZoneDto> source;

    [ObservableProperty]
    private Pagination pagination;

    private bool _isLoading = false;

    public AdminZoneViewModel(IZoneDataService zoneDataService)
    {
        _zoneDataService = zoneDataService;
        Pagination = Pagination ?? new Pagination
        {
            TotalItems = 0,
            TotalPages = 0,
            CurrentPage = 1,
        };
        Pagination.PropertyChanged += Pagination_PropertyChanged;
    }

    public async void OnNavigatedTo(object parameter)
    {
        ZoneCollectionQueryParams query = new ZoneCollectionQueryParams
        {
            Page = Pagination.CurrentPage,
            Size = Pagination.PageSize,
        };
        await LoadDataAsync(query);
    }

    public void OnNavigatedFrom()
    {
    }

    private void Pagination_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (!_isLoading && (e.PropertyName == nameof(Pagination.CurrentPage) || e.PropertyName == nameof(Pagination.PageSize)))
        {
            ZoneCollectionQueryParams query = new ZoneCollectionQueryParams();

            query.OrderDirection = "asc";
            query.Page = Pagination.CurrentPage;
            query.Size = Pagination.PageSize;

            _ = LoadDataAsync(query);
        }
    }

    private async Task LoadDataAsync(ZoneCollectionQueryParams query)
    {
        _isLoading = true;
        var result = await _zoneDataService.GetGridDataAsync(query);

        Source = new ObservableCollection<ZoneDto>(result.Data);
        Pagination.TotalItems = result.TotalItems;
        Pagination.TotalPages = result.TotalPages;
        Pagination.CurrentPage = result.CurrentPage;
        _isLoading = false;
        Console.WriteLine(Pagination);
    }

    public async Task<bool> DeleteZoneAsync(int zoneId)
    {
        var result = await _zoneDataService.DeleteZoneAsync(zoneId);
        var success = result.Success;
        await LoadDataAsync(new ZoneCollectionQueryParams());
        return success;
    }

    public void Dispose()
    {
        if (Pagination != null)
        {
            Pagination.PropertyChanged -= Pagination_PropertyChanged;
        }
    }
}