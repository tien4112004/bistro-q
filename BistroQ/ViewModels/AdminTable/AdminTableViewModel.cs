using BistroQ.Contracts.Services;
using BistroQ.Contracts.ViewModels;
using BistroQ.Core.Dtos.Tables;
using BistroQ.Core.Models;
using BistroQ.Services;
using BistroQ.ViewModels.AdminTable;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace BistroQ.ViewModels;

public partial class AdminTableViewModel : ObservableRecipient, INavigationAware, IDisposable
{
    private readonly IAdminTableService _adminTableService;
    private readonly INavigationService _navigationService;
    [ObservableProperty]
    private bool _isLoading = true;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor("EditCommand")]
    [NotifyCanExecuteChangedFor("DeleteCommand")]
    private TableDto? _selectedTable;

    [ObservableProperty]
    private ObservableCollection<TableDto> _source = new();

    [ObservableProperty]
    private Pagination _pagination;

    [ObservableProperty]
    private string _searchText;

    public IRelayCommand AddCommand { get; }
    public IRelayCommand EditCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }
    public ICommand SortCommand { get; }
    public ICommand SearchCommand { get; }

    public AdminTableViewModel(IAdminTableService adminTableService, INavigationService navigationService)
    {
        _adminTableService = adminTableService;
        _navigationService = navigationService;
        _pagination = new Pagination
        {
            TotalItems = 0,
            TotalPages = 1,
            CurrentPage = 1,
            PageSize = 10
        };
        _pagination.PropertyChanged += Pagination_PropertyChanged;

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

        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync(TableCollectionQueryParams query = null)
    {
        try
        {
            _isLoading = true;

            var _query = query ?? new TableCollectionQueryParams()
            {
                OrderDirection = "asc",
                Page = _pagination.CurrentPage,
                Size = _pagination.PageSize
            };

            var result = await _adminTableService.GetTablesAsync(_query);

            Source = new ObservableCollection<TableDto>(result.Data);
            _pagination.TotalItems = result.TotalItems;
            _pagination.TotalPages = result.TotalPages;
            _pagination.CurrentPage = result.CurrentPage;
        }
        catch (ServiceException ex)
        {
            await _adminTableService.ShowErrorDialog(ex.Message, App.MainWindow.Content.XamlRoot);
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
            var result = await _adminTableService.ShowConfirmDeleteDialog(xamlRoot);
            if (result != ContentDialogResult.Primary) return;

            var success = await _adminTableService.DeleteTableAsync(SelectedTable.TableId.Value);
            if (success)
            {
                await _adminTableService.ShowSuccessDialog("Table deleted successfully.", xamlRoot);
                Source.Remove(SelectedTable);
                SelectedTable = null;
                await LoadDataAsync();
            }
            else
            {
                await _adminTableService.ShowErrorDialog("Failed to delete table.", xamlRoot);
            }
        }
        catch (ServiceException ex)
        {
            await _adminTableService.ShowErrorDialog(ex.Message, App.MainWindow.Content.XamlRoot);
        }
    }

    // sort
    private void ExecuteSortCommand((string column, string direction) sortParams)
    {
        var (column, direction) = sortParams;
        var query = new TableCollectionQueryParams
        {
            OrderBy = column,
            OrderDirection = direction,
            Page = 1,
            Size = _pagination.PageSize
        };
        _ = LoadDataAsync(query);
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

    // search
    private void ExecuteSearchCommand()
    {
        // Implement the search logic here
        var query = new TableCollectionQueryParams
        {
            //Name = SearchText,
            //Page = 1,
            //Size = _pagination.PageSize
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
