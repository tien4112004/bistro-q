using AutoMapper;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.AdminAccount;
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
/// ViewModel for managing admin accounts, including listing, adding, editing, and deleting accounts.
/// Handles data grid operations, pagination, and navigation between admin account pages.
/// </summary>
/// <remarks>
/// Implements multiple interfaces for navigation, messaging, and resource cleanup:
/// - ObservableRecipient for MVVM pattern
/// - INavigationAware for page navigation
/// - IDisposable for cleanup
/// - IRecipient for handling pagination messages
/// </remarks>
public partial class AdminAccountViewModel :
    ObservableRecipient,
    INavigationAware,
    IDisposable,
    IRecipient<PageSizeChangedMessage>,
    IRecipient<CurrentPageChangedMessage>
{
    #region Private Fields
    private readonly IDialogService _dialogService;
    private readonly IMapper _mapper;
    private readonly IAccountDataService _accountDataService;
    private readonly INavigationService _navigationService;
    private readonly IMessenger _messenger;
    private bool _isDisposed = false;
    #endregion

    #region Observable Properties
    /// <summary>
    /// The state container for admin account management, including selected account,
    /// query parameters, and loading state.
    /// </summary>
    [ObservableProperty]
    private AdminAccountState state = new();
    #endregion

    #region Commands
    /// <summary>
    /// Command to navigate to the add account page.
    /// </summary>
    public IRelayCommand AddCommand { get; }

    /// <summary>
    /// Command to navigate to the edit account page.
    /// </summary>
    public IRelayCommand EditCommand { get; }

    /// <summary>
    /// Command to delete the selected account.
    /// </summary>
    public IAsyncRelayCommand DeleteCommand { get; }

    /// <summary>
    /// Command to sort the accounts grid.
    /// </summary>
    public ICommand SortCommand { get; }

    /// <summary>
    /// Command to search accounts.
    /// </summary>
    public ICommand SearchCommand { get; }
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the AdminAccountViewModel class.
    /// </summary>
    /// <param name="navigationService">Service for handling navigation between pages.</param>
    /// <param name="accountDataService">Service for account data operations.</param>
    /// <param name="dialogService">Service for showing dialogs.</param>
    /// <param name="mapper">AutoMapper instance for object mapping.</param>
    /// <param name="messenger">Messenger for handling inter-component communication.</param>
    public AdminAccountViewModel(
        INavigationService navigationService,
        IAccountDataService accountDataService,
        IDialogService dialogService,
        IMapper mapper,
        IMessenger messenger)
    {
        _navigationService = navigationService;
        _accountDataService = accountDataService;
        _mapper = mapper;
        _messenger = messenger;
        _dialogService = dialogService;

        State.PropertyChanged += StatePropertyChanged;

        AddCommand = new RelayCommand(NavigateToAddPage);
        EditCommand = new RelayCommand(NavigateToEditPage, () => State.CanEdit);
        DeleteCommand = new AsyncRelayCommand(DeleteSelectedAccountAsync, () => State.CanDelete);
        SortCommand = new RelayCommand<(string column, string direction)>(ExecuteSortCommand);
        SearchCommand = new RelayCommand(ExecuteSearchCommand);

        _messenger.RegisterAll(this);
    }
    #endregion

    #region Event Handlers
    /// <summary>
    /// Handles property changes in the AdminAccountState.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments containing the property name.</param>
    private void StatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(State.SelectedAccount))
        {
            EditCommand.NotifyCanExecuteChanged();
            DeleteCommand.NotifyCanExecuteChanged();
        }
    }
    #endregion

    #region Navigation Methods
    /// <summary>
    /// Handles navigation to this page.
    /// </summary>
    /// <param name="parameter">Navigation parameter.</param>
    public async Task OnNavigatedTo(object parameter)
    {
        await LoadDataAsync();
    }

    /// <summary>
    /// Handles navigation from this page.
    /// </summary>
    public Task OnNavigatedFrom()
    {
        Dispose();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Navigates to the add account page.
    /// </summary>
    private void NavigateToAddPage() =>
        _navigationService.NavigateTo(typeof(AdminAccountAddPageViewModel).FullName);

    /// <summary>
    /// Navigates to the edit account page with the selected account.
    /// </summary>
    private void NavigateToEditPage()
    {
        if (State.SelectedAccount?.UserId != null)
        {
            _navigationService.NavigateTo(typeof(AdminAccountEditPageViewModel).FullName, State.SelectedAccount);
            Dispose();
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Loads or reloads the accounts data grid.
    /// </summary>
    private async Task LoadDataAsync()
    {
        if (State.IsLoading || _isDisposed) return;
        try
        {
            State.IsLoading = true;

            var result = await _accountDataService.GetAccountsAsync(State.Query);

            var accounts = _mapper.Map<IEnumerable<AccountViewModel>>(result.Data);
            State.Source = new ObservableCollection<AccountViewModel>(accounts);
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
    /// Deletes the selected account after confirmation.
    /// </summary>
    private async Task DeleteSelectedAccountAsync()
    {
        if (State.SelectedAccount?.UserId == null) return;

        try
        {
            var result = await _dialogService.ShowConfirmDeleteDialog();
            if (result != ContentDialogResult.Primary) return;

            var success = await _accountDataService.DeleteAccountAsync(State.SelectedAccount.UserId);
            if (success)
            {
                await _dialogService.ShowSuccessDialog("Account deleted successfully.", "Success");
                State.Source.Remove(State.SelectedAccount);
                State.SelectedAccount = null;
                await LoadDataAsync();
            }
            else
            {
                await _dialogService.ShowErrorDialog("Failed to delete account.", "Error");
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
    /// Executes the search command.
    /// </summary>
    private void ExecuteSearchCommand()
    {
        State.ReturnToFirstPage();
        _ = LoadDataAsync();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Handles the sorting event of the admin account data grid.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments containing the column information.</param>
    public void AdminAccountDataGrid_Sorting(object sender, DataGridColumnEventArgs e)
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
    /// Handles the current page changed message.
    /// </summary>
    /// <param name="message">Message containing the new current page.</param>
    public void Receive(CurrentPageChangedMessage message)
    {
        State.Query.Page = message.NewCurrentPage;
        _ = LoadDataAsync();
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
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
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
    #endregion
}