using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.Services;
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
    IDisposable
{
    private readonly IAdminDialogService _adminZoneDialogService;
    private readonly INavigationService _navigationService;
    private readonly IZoneDataService _zoneDataService;
    private readonly IMapper _mapper;
    private readonly IMessenger _messenger;

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
        IMapper mapper,
        IMessenger messenger)
    {
        _navigationService = navigationService;
        _zoneDataService = zoneDataService;
        _mapper = mapper;
        _adminZoneDialogService = new AdminZoneDialogService();

        State.PropertyChanged += StatePropertyChanged;

        AddCommand = new RelayCommand(NavigateToAddPage);
        EditCommand = new RelayCommand(NavigateToEditPage, () => State.CanEdit);
        DeleteCommand = new AsyncRelayCommand(DeleteSelectedZoneAsync, () => State.CanDelete);
        SortCommand = new RelayCommand<(string column, string direction)>(ExecuteSortCommand);
        SearchCommand = new RelayCommand(ExecuteSearchCommand);

        _messenger = messenger;
        _messenger.Register<PageSizeChangedMessage>(this, async (r, m) =>
        {
            State.Query.Size = m.NewPageSize;
            await LoadDataAsync();
        });

        _messenger.Register<CurrentPageChangedMessage>(this, async (r, m) =>
        {
            State.Query.Page = m.NewCurrentPage;
            await LoadDataAsync();
        });
    }

    private void StatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(State.SelectedZone))
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
        State.SelectedZone = null;
    }

    private async Task LoadDataAsync()
    {
        try
        {
            State.IsLoading = true;
            var result = await _zoneDataService.GetZonesAsync(State.Query);

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
            await _adminZoneDialogService.ShowErrorDialog(ex.Message);
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
            var result = await _adminZoneDialogService.ShowConfirmDeleteDialog();
            if (result != ContentDialogResult.Primary) return;

            var success = await _zoneDataService.DeleteZoneAsync(State.SelectedZone.ZoneId.Value);
            if (success)
            {
                await _adminZoneDialogService.ShowSuccessDialog("Zone deleted successfully.");
                State.Source.Remove(State.SelectedZone);
                State.SelectedZone = null;
                await LoadDataAsync();
            }
            else
            {
                await _adminZoneDialogService.ShowErrorDialog("Failed to delete zone.");
            }
        }
        catch (Exception ex)
        {
            await _adminZoneDialogService.ShowErrorDialog(ex.Message);
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
        if (State != null)
        {
            State.PropertyChanged -= StatePropertyChanged;
        }
    }
}