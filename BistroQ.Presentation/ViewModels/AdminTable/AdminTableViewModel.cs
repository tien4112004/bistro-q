using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Domain.Dtos.Tables;
using BistroQ.Domain.Models;
using BistroQ.Presentation.Services;
using BistroQ.Presentation.ViewModels.AdminTable;
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
    private Pagination _pagination;

    [ObservableProperty]
    private string _searchText;

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

            var result = await _tableDataService.GetGridDataAsync(_query);

            var tables = _mapper.Map<IEnumerable<TableViewModel>>(result.Data);
            Source = new ObservableCollection<TableViewModel>(tables);
            _pagination.TotalItems = result.TotalItems;
            _pagination.TotalPages = result.TotalPages;
            _pagination.CurrentPage = result.CurrentPage;
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