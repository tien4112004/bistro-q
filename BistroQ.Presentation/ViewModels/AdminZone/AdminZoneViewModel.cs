using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Domain.Models;
using BistroQ.Presentation.Services;
using BistroQ.Presentation.ViewModels.AdminZone;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Presentation.ViewModels.Models;

namespace BistroQ.Presentation.ViewModels;

public partial class AdminZoneViewModel : ObservableRecipient, INavigationAware, IDisposable
{
    private readonly IAdminZoneDialogService _adminZoneDialogService;
    private readonly INavigationService _navigationService;
    private readonly IZoneDataService _zoneDataService;
    private readonly IMapper _mapper;
    private bool _isLoading;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor("EditCommand")]
    [NotifyCanExecuteChangedFor("DeleteCommand")]
    private ZoneViewModel? _selectedZone;

    [ObservableProperty]
    private ObservableCollection<ZoneViewModel> _source = new();

    [ObservableProperty]
    private Pagination _pagination;

    [ObservableProperty]
    private string _searchText;

    public IRelayCommand AddCommand { get; }
    public IRelayCommand EditCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }
    public ICommand SortCommand { get; }
    public ICommand SearchCommand { get; }

    public AdminZoneViewModel(
        IAdminZoneDialogService adminZoneDialogService, 
        INavigationService navigationService,
        IMapper mapper,
        IZoneDataService zoneDataService)
    {
        _adminZoneDialogService = adminZoneDialogService;
        _navigationService = navigationService;
        _mapper = mapper;
        _zoneDataService = zoneDataService;
        _pagination = new Pagination
        {
            TotalItems = 0,
            TotalPages = 0,
            CurrentPage = 1,
            PageSize = 10
        };
        _pagination.PropertyChanged += Pagination_PropertyChanged;

        AddCommand = new RelayCommand(NavigateToAddPage);
        EditCommand = new RelayCommand(NavigateToEditPage, CanEdit);
        DeleteCommand = new AsyncRelayCommand(DeleteSelectedZoneAsync, CanDelete);
        SortCommand = new RelayCommand<(string column, string direction)>(ExecuteSortCommand);
        SearchCommand = new RelayCommand(ExecuteSearchCommand);
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

    private async Task LoadDataAsync(ZoneCollectionQueryParams query = null)
    {
        try
        {
            _isLoading = true;

            var _query = query ?? new ZoneCollectionQueryParams()
            {
                OrderDirection = "asc",
                Page = _pagination.CurrentPage,
                Size = _pagination.PageSize
            };

            var result = await _zoneDataService.GetZonesAsync(_query);

            Source = new ObservableCollection<ZoneViewModel>(_mapper.Map<IEnumerable<ZoneViewModel>>(result.Data));
            _pagination.TotalItems = result.TotalItems;
            _pagination.TotalPages = result.TotalPages;
            _pagination.CurrentPage = result.CurrentPage;
        }
        catch (Exception ex)
        {
            await _adminZoneDialogService.ShowErrorDialog(ex.Message, App.MainWindow.Content.XamlRoot);
        }
        finally
        {
            _isLoading = false;
        }
    }

    // buttons
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
            var result = await _adminZoneDialogService.ShowConfirmDeleteDialog(xamlRoot);
            if (result != ContentDialogResult.Primary) return;

            var success = await _zoneDataService.DeleteZoneAsync(SelectedZone.ZoneId.Value);
            if (success)
            {
                await _adminZoneDialogService.ShowSuccessDialog("Zone deleted successfully.", xamlRoot);
                Source.Remove(SelectedZone);
                SelectedZone = null;
                await LoadDataAsync();
            }
            else
            {
                await _adminZoneDialogService.ShowErrorDialog("Failed to delete zone.", xamlRoot);
            }
        }
        catch (Exception ex)
        {
            await _adminZoneDialogService.ShowErrorDialog(ex.Message, App.MainWindow.Content.XamlRoot);
        }
    }

    // sort
    private void ExecuteSortCommand((string column, string direction) sortParams)
    {
        var (column, direction) = sortParams;
        var query = new ZoneCollectionQueryParams
        {
            OrderBy = column,
            OrderDirection = direction,
            Page = 1,
            Size = _pagination.PageSize
        };
        _ = LoadDataAsync(query);
    }

    public void AdminZoneDataGrid_Sorting(object sender, DataGridColumnEventArgs e)
    {
        var column = e.Column;
        var sortDirection = column.SortDirection == null || column.SortDirection == DataGridSortDirection.Descending
            ? "asc"
            : "des";

        column.SortDirection = sortDirection == "asc"
            ? DataGridSortDirection.Ascending
            : DataGridSortDirection.Descending;

        var sortParams = (column.Tag.ToString(), sortDirection);
        SortCommand.Execute(sortParams);
    }

    // search
    private void ExecuteSearchCommand()
    {
        // Implement the search logic here
        var query = new ZoneCollectionQueryParams
        {
            Name = SearchText,
            Page = 1,
            Size = _pagination.PageSize
        };
        _ = LoadDataAsync(query);
    }

    private void Control2_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion != null)
        {
            SearchText = args.ChosenSuggestion.ToString();
        }
        else
        {
            SearchText = args.QueryText;
        }

        SearchCommand.Execute(null);
    }

    private void Control2_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        sender.Text = args.SelectedItem.ToString();
    }

    public void Dispose()
    {
        if (_pagination != null)
        {
            _pagination.PropertyChanged -= Pagination_PropertyChanged;
        }
    }
}