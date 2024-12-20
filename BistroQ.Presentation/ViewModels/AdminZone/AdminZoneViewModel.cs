using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.AdminZone;
using BistroQ.Presentation.ViewModels.Models;
using BistroQ.Presentation.ViewModels.States;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels;

public partial class AdminZoneViewModel :
    ObservableRecipient,
    INavigationAware,
    IDisposable,
    IRecipient<PageSizeChangedMessage>,
    IRecipient<CurrentPageChangedMessage>
{
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private readonly IZoneDataService _zoneDataService;
    private readonly IMapper _mapper;
    private readonly IMessenger _messenger;
    private bool _isDisposed = false;

    [ObservableProperty]
    private AdminZoneState state = new();

    public IRelayCommand AddCommand { get; }
    public IRelayCommand EditCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }
    public ICommand SortCommand { get; }
    public ICommand SearchCommand { get; }

    public AdminZoneViewModel(
        INavigationService navigationService,
        IZoneDataService zoneDataService,
        IDialogService dialogService,
        IMapper mapper,
        IMessenger messenger)
    {
        _navigationService = navigationService;
        _zoneDataService = zoneDataService;
        _mapper = mapper;
        _dialogService = dialogService;

        State.PropertyChanged += StatePropertyChanged;

        AddCommand = new RelayCommand(NavigateToAddPage);
        EditCommand = new RelayCommand(NavigateToEditPage, () => State.CanEdit);
        DeleteCommand = new AsyncRelayCommand(DeleteSelectedZoneAsync, () => State.CanDelete);
        SortCommand = new RelayCommand<(string column, string direction)>(ExecuteSortCommand);
        SearchCommand = new RelayCommand(ExecuteSearchCommand);

        _messenger = messenger;
        _messenger.RegisterAll(this);
    }

    private void StatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(State.SelectedZone))
        {
            EditCommand.NotifyCanExecuteChanged();
            DeleteCommand.NotifyCanExecuteChanged();
        }
    }

    public void OnNavigatedTo(object parameter)
    {
        _ = LoadDataAsync();
    }

    public void OnNavigatedFrom()
    {
        Dispose();
    }

    private async Task LoadDataAsync()
    {
        if (State.IsLoading || _isDisposed) return;
        try
        {
            State.IsLoading = true;
            var result = await _zoneDataService.GetGridDataAsync(State.Query);

            var zones = _mapper.Map<IEnumerable<ZoneViewModel>>(result.Data);
            State.Source = new ObservableCollection<ZoneViewModel>(zones);
            _messenger.Send(new PaginationChangedMessage(
                result.TotalItems,
                result.CurrentPage,
                result.TotalPages
             ));
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorDialog(ex.Message, "Error");
        }
        finally
        {
            State.IsLoading = false;
        }
    }

    private void NavigateToAddPage() =>
        _navigationService.NavigateTo(typeof(AdminZoneAddPageViewModel).FullName);

    private void NavigateToEditPage()
    {
        if (State.SelectedZone?.ZoneId != null)
        {
            _navigationService.NavigateTo(typeof(AdminZoneEditPageViewModel).FullName, State.SelectedZone);
        }
    }

    private async Task DeleteSelectedZoneAsync()
    {
        if (State.SelectedZone?.ZoneId == null) return;

        try
        {
            var result = await _dialogService.ShowConfirmDeleteDialog();
            if (result != ContentDialogResult.Primary) return;

            var success = await _zoneDataService.DeleteZoneAsync(State.SelectedZone.ZoneId.Value);
            if (success)
            {
                await _dialogService.ShowSuccessDialog("Zone deleted successfully.", "Success");
                State.Source.Remove(State.SelectedZone);
                State.SelectedZone = null;
                await LoadDataAsync();
            }
            else
            {
                await _dialogService.ShowErrorDialog("Failed to delete zone.", "Error");
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorDialog(ex.Message, "Error");
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

    public void AdminZoneDataGrid_Sorting(object sender, DataGridColumnEventArgs e)
    {
        var dataGrid = sender as DataGrid;
        var column = e.Column;
        var sortDirection = column.SortDirection == null || column.SortDirection == DataGridSortDirection.Descending
            ? "asc"
            : "des";

        foreach (var col in dataGrid.Columns)
        {
            if (col != column)
            {
                col.SortDirection = null;
            }
        }

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
        if (_isDisposed) return;
        _isDisposed = true;
        if (State != null)
        {
            State.PropertyChanged -= StatePropertyChanged;
        }

        _messenger.UnregisterAll(this);
    }

    public void Receive(PageSizeChangedMessage message)
    {
        State.Query.Size = message.NewPageSize;
        State.ReturnToFirstPage();
        _ = LoadDataAsync();
    }

    public void Receive(CurrentPageChangedMessage message)
    {
        State.Query.Page = message.NewCurrentPage;
        _ = LoadDataAsync();
    }
}