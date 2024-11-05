using BistroQ.Contracts.Services;
using BistroQ.Contracts.ViewModels;
using BistroQ.Core.Dtos.Zones;
using BistroQ.Core.Models;
using BistroQ.Services;
using BistroQ.ViewModels.AdminZone;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BistroQ.ViewModels;

public partial class AdminZoneViewModel : ObservableRecipient, INavigationAware, IDisposable
{
    private readonly IAdminZoneService _adminZoneService;
    private readonly INavigationService _navigationService;
    private bool _isLoading;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor("EditCommand")]
    [NotifyCanExecuteChangedFor("DeleteCommand")]
    private ZoneDto? selectedZone;

    [ObservableProperty]
    private ObservableCollection<ZoneDto> source = new();

    [ObservableProperty]
    private Pagination pagination;

    public IRelayCommand AddCommand { get; }
    public IRelayCommand EditCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }

    public AdminZoneViewModel(IAdminZoneService adminZoneService, INavigationService navigationService)
    {
        _adminZoneService = adminZoneService;
        _navigationService = navigationService;
        pagination = new Pagination
        {
            TotalItems = 0,
            TotalPages = 0,
            CurrentPage = 1,
            PageSize = 10
        };
        pagination.PropertyChanged += Pagination_PropertyChanged;

        AddCommand = new RelayCommand(NavigateToAddPage);
        EditCommand = new RelayCommand(NavigateToEditPage, CanEdit);
        DeleteCommand = new AsyncRelayCommand(DeleteSelectedZoneAsync, CanDelete);
    }

    public async void OnNavigatedTo(object parameter)
    {
        await LoadDataAsync();
    }

    public void OnNavigatedFrom()
    {
        SelectedZone = null;
    }

    private void Pagination_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_isLoading ||
            (e.PropertyName != nameof(Pagination.CurrentPage) &&
             e.PropertyName != nameof(Pagination.PageSize)))
        {
            return;
        }

        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            _isLoading = true;

            var query = new ZoneCollectionQueryParams
            {
                OrderDirection = "asc",
                Page = pagination.CurrentPage,
                Size = pagination.PageSize
            };

            var (data, totalItems, totalPages, currentPage) =
                await _adminZoneService.GetZonesAsync(query);

            Source = new ObservableCollection<ZoneDto>(data);
            pagination.TotalItems = totalItems;
            pagination.TotalPages = totalPages;
            pagination.CurrentPage = currentPage;
        }
        catch (ServiceException ex)
        {
            await _adminZoneService.ShowErrorDialog(ex.Message, App.MainWindow.Content.XamlRoot);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void NavigateToAddPage() =>
        _navigationService.NavigateTo(typeof(AdminZoneAddPageViewModel).FullName);

    private bool CanEdit() => SelectedZone != null;

    private void NavigateToEditPage()
    {
        if (SelectedZone?.ZoneId != null)
        {
            _navigationService.NavigateTo(typeof(AdminZoneEditPageViewModel).FullName, SelectedZone);
        }
    }

    private bool CanDelete() => SelectedZone != null;

    private async Task DeleteSelectedZoneAsync()
    {
        if (SelectedZone?.ZoneId == null) return;

        try
        {
            var xamlRoot = App.MainWindow.Content.XamlRoot;
            var result = await _adminZoneService.ShowConfirmDeleteDialog(xamlRoot);
            if (result != ContentDialogResult.Primary) return;

            var success = await _adminZoneService.DeleteZoneAsync(SelectedZone.ZoneId.Value);
            if (success)
            {
                await _adminZoneService.ShowSuccessDialog("Zone deleted successfully.", xamlRoot);
                Source.Remove(SelectedZone);
                SelectedZone = null;
                await LoadDataAsync();
            }
            else
            {
                await _adminZoneService.ShowErrorDialog("Failed to delete zone.", xamlRoot);
            }
        }
        catch (ServiceException ex)
        {
            await _adminZoneService.ShowErrorDialog(ex.Message, App.MainWindow.Content.XamlRoot);
        }
    }

    public void Dispose()
    {
        if (pagination != null)
        {
            pagination.PropertyChanged -= Pagination_PropertyChanged;
        }
    }
}