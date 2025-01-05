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

/// <summary>
/// ViewModel for managing zones in the admin interface.
/// Handles listing, adding, editing, and deleting zones with support for pagination and sorting.
/// </summary>
/// <remarks>
/// Implements multiple interfaces for functionality:
/// - ObservableRecipient for MVVM pattern
/// - INavigationAware for page navigation
/// - IDisposable for resource cleanup
/// - IRecipient for handling pagination messages
/// </remarks>
public partial class AdminZoneViewModel :
    ObservableRecipient,
    INavigationAware,
    IDisposable,
    IRecipient<PageSizeChangedMessage>,
    IRecipient<CurrentPageChangedMessage>
{
    #region Private Fields
    /// <summary>
    /// Service for displaying dialog messages to the user.
    /// </summary>
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Service for handling navigation between pages.
    /// </summary>
    private readonly INavigationService _navigationService;

    /// <summary>
    /// Service for managing zone data operations.
    /// </summary>
    private readonly IZoneDataService _zoneDataService;

    /// <summary>
    /// Service for object mapping operations.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Service for handling messaging between components.
    /// </summary>
    private readonly IMessenger _messenger;

    /// <summary>
    /// Flag indicating whether the view model has been disposed.
    /// </summary>
    private bool _isDisposed = false;

    /// <summary>
    /// The state container for zone management, including selected zone,
    /// query parameters, and loading state.
    /// </summary>
    [ObservableProperty]
    private AdminZoneState state = new();
    #endregion

    #region Public Properties
    /// <summary>
    /// Command to navigate to the add zone page.
    /// </summary>
    public IRelayCommand AddCommand { get; }

    /// <summary>
    /// Command to navigate to the edit zone page.
    /// </summary>
    public IRelayCommand EditCommand { get; }

    /// <summary>
    /// Command to delete the selected zone.
    /// </summary>
    public IAsyncRelayCommand DeleteCommand { get; }

    /// <summary>
    /// Command to sort the zones grid.
    /// </summary>
    public ICommand SortCommand { get; }

    /// <summary>
    /// Command to search zones.
    /// </summary>
    public ICommand SearchCommand { get; }
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the AdminZoneViewModel class.
    /// </summary>
    /// <param name="navigationService">Service for navigation operations.</param>
    /// <param name="zoneDataService">Service for zone data operations.</param>
    /// <param name="dialogService">Service for displaying dialogs.</param>
    /// <param name="mapper">Service for object mapping.</param>
    /// <param name="messenger">Service for messaging between components.</param>
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
    #endregion

    #region Event Handlers
    /// <summary>
    /// Handles property changes in the ZoneState.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments containing the property name.</param>
    private void StatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(State.SelectedZone))
        {
            EditCommand.NotifyCanExecuteChanged();
            DeleteCommand.NotifyCanExecuteChanged();
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Handles navigation to this page.
    /// </summary>
    /// <param name="parameter">Navigation parameter.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task OnNavigatedTo(object parameter)
    {
        await LoadDataAsync();
    }

    /// <summary>
    /// Handles navigation from this page.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task OnNavigatedFrom()
    {
        Dispose();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles the sorting event of the zone data grid.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments containing the column information.</param>
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

    /// <summary>
    /// Performs cleanup of resources used by the ViewModel.
    /// </summary>
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

    /// <summary>
    /// Handles the page size changed message.
    /// </summary>
    /// <param name="message">Message containing the new page size.</param>
    public void Receive(PageSizeChangedMessage message)
    {
        State.Query.Size = message.NewPageSize;
        State.ReturnToFirstPage();
        _ = LoadDataAsync();
    }

    /// <summary>
    /// Handles the current page changed message.
    /// </summary>
    /// <param name="message">Message containing the new current page.</param>
    public void Receive(CurrentPageChangedMessage message)
    {
        State.Query.Page = message.NewCurrentPage;
        _ = LoadDataAsync();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Loads or reloads the zones data grid.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Navigates to the add zone page.
    /// </summary>
    private void NavigateToAddPage() =>
        _navigationService.NavigateTo(typeof(AdminZoneAddPageViewModel).FullName);

    /// <summary>
    /// Navigates to the edit zone page with the selected zone.
    /// </summary>
    private void NavigateToEditPage()
    {
        if (State.SelectedZone?.ZoneId != null)
        {
            _navigationService.NavigateTo(typeof(AdminZoneEditPageViewModel).FullName, State.SelectedZone);
        }
    }

    /// <summary>
    /// Deletes the selected zone after confirmation.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Executes the sort command with the specified parameters.
    /// </summary>
    /// <param name="sortParams">Tuple containing column name and sort direction.</param>
    private void ExecuteSortCommand((string column, string direction) sortParams)
    {
        var (column, direction) = sortParams;
        State.Query.OrderBy = column;
        State.Query.OrderDirection = direction;
        State.ReturnToFirstPage();
        _ = LoadDataAsync();
    }

    /// <summary>
    /// Executes the search command and reloads the data.
    /// </summary>
    private void ExecuteSearchCommand()
    {
        State.ReturnToFirstPage();
        _ = LoadDataAsync();
    }
    #endregion
}