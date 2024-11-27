using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.ViewModels.AdminTable;
using BistroQ.Presentation.ViewModels.Models;
using BistroQ.Presentation.ViewModels.States;
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

    [ObservableProperty]
    private AdminTableState state = new();
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

        State.PropertyChanged += StatePropertyChanged;
        State.Pagination.PropertyChanged += Pagination_PropertyChanged;

        AddCommand = new RelayCommand(NavigateToAddPage);
        EditCommand = new RelayCommand(NavigateToEditPage, () => State.CanEdit);
        DeleteCommand = new AsyncRelayCommand(DeleteSelectedTableAsync, () => State.CanDelete);
        SortCommand = new RelayCommand<(string column, string direction)>(ExecuteSortCommand);
        SearchCommand = new RelayCommand(ExecuteSearchCommand);
    }

    private void StatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(State.SelectedTable))
        {
            EditCommand.NotifyCanExecuteChanged();
            DeleteCommand.NotifyCanExecuteChanged();
        }
    }

    public async void OnNavigatedTo(object parameter)
    {
        await LoadDataAsync();
    }

    public void OnNavigatedFrom()
    {
        State.SelectedTable = null;
    }

    private void Pagination_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (State.IsLoading ||
            (e.PropertyName != nameof(State.Pagination.CurrentPage) &&
             e.PropertyName != nameof(State.Pagination.PageSize)))
        {
            return;
        }

        State.Query.Page = State.Pagination.CurrentPage;
        State.Query.Size = State.Pagination.PageSize;
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            State.IsLoading = true;

            var result = await _tableDataService.GetGridDataAsync(State.Query);

            var tables = _mapper.Map<IEnumerable<TableViewModel>>(result.Data);
            State.Source = new ObservableCollection<TableViewModel>(tables);
            State.Pagination.TotalItems = result.TotalItems;
            State.Pagination.TotalPages = result.TotalPages;
            State.Pagination.CurrentPage = result.CurrentPage;
        }
        catch (Exception ex)
        {
            await _adminTableDialogService.ShowErrorDialog(ex.Message, App.MainWindow.Content.XamlRoot);
        }
        finally
        {
            State.IsLoading = false;
        }
    }

    private void NavigateToAddPage() =>
        _navigationService.NavigateTo(typeof(AdminTableAddPageViewModel).FullName);

    private void NavigateToEditPage()
    {
        if (State.SelectedTable?.TableId != null)
        {
            _navigationService.NavigateTo(typeof(AdminTableEditPageViewModel).FullName, State.SelectedTable);
        }
    }

    private async Task DeleteSelectedTableAsync()
    {
        if (State.SelectedTable?.TableId == null) return;

        try
        {
            var xamlRoot = App.MainWindow.Content.XamlRoot;
            var result = await _adminTableDialogService.ShowConfirmDeleteDialog(xamlRoot);
            if (result != ContentDialogResult.Primary) return;

            var success = await _tableDataService.DeleteTableAsync(State.SelectedTable.TableId.Value);
            if (success)
            {
                await _adminTableDialogService.ShowSuccessDialog("Table deleted successfully.", xamlRoot);
                State.Source.Remove(State.SelectedTable);
                State.SelectedTable = null;
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

    private void ExecuteSortCommand((string column, string direction) sortParams)
    {
        var (column, direction) = sortParams;
        State.Query.OrderBy = column;
        State.Query.OrderDirection = direction;
        State.ReturnToFirstPage();
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
        State.ReturnToFirstPage();
        _ = LoadDataAsync();
    }

    public void Dispose()
    {
        if (State.Pagination != null)
        {
            State.Pagination.PropertyChanged -= Pagination_PropertyChanged;
        }
    }
}