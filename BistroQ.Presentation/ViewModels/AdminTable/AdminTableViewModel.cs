using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Tables;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.ViewModels.AdminTable;
using BistroQ.Presentation.ViewModels.Commons;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels;

public partial class AdminTableViewModel : ObservableRecipient, INavigationAware, IDisposable
{
    private readonly IAdminTableDialogService _adminTableDialogService;
    private readonly IMapper _mapper;
    private readonly ITableDataService _tableDataService;
    private readonly INavigationService _navigationService;
    private bool _isLoading;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor("EditCommand")]
    [NotifyCanExecuteChangedFor("DeleteCommand")]
    private TableViewModel? _selectedTable;

    [ObservableProperty]
    private ObservableCollection<TableViewModel> _source = new();

    [ObservableProperty]
    private PaginationViewModel _pagination;

    [ObservableProperty]
    private TableCollectionQueryParams _query = new();

    public string SearchText
    {
        get => Query.ZoneName ?? string.Empty;
        set
        {
            if (Query.ZoneName != value)
            {
                Query.ZoneName = value;
                OnPropertyChanged();
            }
        }
    }

    public IRelayCommand AddCommand { get; }
    public IRelayCommand EditCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }
    public ICommand SortCommand { get; }
    public ICommand SearchCommand { get; }

    public AdminTableViewModel(
        INavigationService navigationService,
        IAdminTableDialogService adminTableDialogService,
        ITableDataService tableDataService,
        IMapper mapper)
    {
        _navigationService = navigationService;
        _adminTableDialogService = adminTableDialogService;
        _tableDataService = tableDataService;
        _mapper = mapper;
        Pagination = new PaginationViewModel
        {
            TotalItems = 0,
            TotalPages = 0,
            CurrentPage = 1,
            PageSize = 10
        };
        Pagination.PropertyChanged += Pagination_PropertyChanged;

        AddCommand = new RelayCommand(NavigateToAddPage);
        EditCommand = new RelayCommand(NavigateToEditPage, CanEdit);
        DeleteCommand = new AsyncRelayCommand(DeleteSelectedTableAsync, CanDelete);
        SortCommand = new RelayCommand<(string column, string direction)>(ExecuteSortCommand);
        SearchCommand = new RelayCommand(ExecuteSearchCommand);
    }

    public async void OnNavigatedTo(object parameter)
    {
        await LoadDataAsync();
    }

    public void OnNavigatedFrom()
    {
        SelectedTable = null;
    }

    private void Pagination_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_isLoading ||
            (e.PropertyName != nameof(Pagination.CurrentPage) &&
             e.PropertyName != nameof(Pagination.PageSize)))
        {
            return;
        }

        Query.Page = Pagination.CurrentPage;
        Query.Size = Pagination.PageSize;
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            _isLoading = true;

            var result = await _tableDataService.GetGridDataAsync(Query);

            var tables = _mapper.Map<IEnumerable<TableViewModel>>(result.Data);
            Source = new ObservableCollection<TableViewModel>(tables);
            Pagination.TotalItems = result.TotalItems;
            Pagination.TotalPages = result.TotalPages;
            Pagination.CurrentPage = result.CurrentPage;
        }
        catch (Exception ex)
        {
            await _adminTableDialogService.ShowErrorDialog(ex.Message, App.MainWindow.Content.XamlRoot);
        }
        finally
        {
            _isLoading = false;
        }
    }

    // buttons
    private void NavigateToAddPage() =>
        _navigationService.NavigateTo(typeof(AdminTableAddPageViewModel).FullName);

    private bool CanEdit() => SelectedTable != null;

    private void NavigateToEditPage()
    {
        if (SelectedTable?.TableId != null)
        {
            _navigationService.NavigateTo(typeof(AdminTableEditPageViewModel).FullName, SelectedTable);
        }
    }

    private bool CanDelete() => SelectedTable != null;

    private async Task DeleteSelectedTableAsync()
    {
        if (SelectedTable?.TableId == null) return;

        try
        {
            var xamlRoot = App.MainWindow.Content.XamlRoot;
            var result = await _adminTableDialogService.ShowConfirmDeleteDialog(xamlRoot);
            if (result != ContentDialogResult.Primary) return;

            var success = await _tableDataService.DeleteTableAsync(SelectedTable.TableId.Value);
            if (success)
            {
                await _adminTableDialogService.ShowSuccessDialog("Table deleted successfully.", xamlRoot);
                Source.Remove(SelectedTable);
                SelectedTable = null;
                await LoadDataAsync();
            }
            else
            {
                await _adminTableDialogService.ShowErrorDialog("Failed to delete table.", xamlRoot);
            }
        }
        catch (Exception ex)
        {
            await _adminTableDialogService.ShowErrorDialog(ex.Message, App.MainWindow.Content.XamlRoot);
        }
    }

    // sort
    private void ExecuteSortCommand((string column, string direction) sortParams)
    {
        var (column, direction) = sortParams;
        Query.OrderBy = column;
        Query.OrderDirection = direction;
        Query.Page = 1;
        Pagination.CurrentPage = 1;
        _ = LoadDataAsync();
    }

    public void AdminTableDataGrid_Sorting(object sender, DataGridColumnEventArgs e)
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

    private void ExecuteSearchCommand()
    {
        Query.Page = 1;
        Pagination.CurrentPage = 1;
        _ = LoadDataAsync();
    }

    public void Dispose()
    {
        if (Pagination != null)
        {
            Pagination.PropertyChanged -= Pagination_PropertyChanged;
        }
    }
}