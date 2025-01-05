using AutoMapper;
using BistroQ.Domain.Contracts.Services.Data;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.AdminProduct;
using BistroQ.Presentation.ViewModels.Models;
using BistroQ.Presentation.ViewModels.States;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels;

/// <summary>
/// ViewModel for managing products in the admin interface.
/// Handles listing, adding, editing, and deleting products with support for pagination and sorting.
/// </summary>
/// <remarks>
/// Implements multiple interfaces for functionality:
/// - ObservableRecipient for MVVM pattern
/// - INavigationAware for page navigation
/// - IDisposable for resource cleanup
/// - IRecipient for handling pagination messages
/// </remarks>
public partial class AdminProductViewModel :
    ObservableRecipient,
    INavigationAware,
    IDisposable,
    IRecipient<PageSizeChangedMessage>,
    IRecipient<CurrentPageChangedMessage>
{
    #region Private Fields
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private readonly IProductDataService _productDataService;
    private readonly IMapper _mapper;
    private readonly IMessenger _messenger;
    private bool _isDisposed = false;
    #endregion

    #region Observable Properties
    /// <summary>
    /// The state container for product management, including selected product,
    /// query parameters, and loading state.
    /// </summary>
    [ObservableProperty]
    private AdminProductState _state = new();
    #endregion

    #region Commands
    /// <summary>
    /// Command to navigate to the add product page.
    /// </summary>
    public IRelayCommand AddCommand { get; }

    /// <summary>
    /// Command to navigate to the edit product page.
    /// </summary>
    public IRelayCommand EditCommand { get; }

    /// <summary>
    /// Command to delete the selected product.
    /// </summary>
    public IAsyncRelayCommand DeleteCommand { get; }

    /// <summary>
    /// Command to sort the products grid.
    /// </summary>
    public ICommand SortCommand { get; }

    /// <summary>
    /// Command to search products.
    /// </summary>
    public ICommand SearchCommand { get; }
    #endregion

    #region Constructor
    public AdminProductViewModel(
        INavigationService navigationService,
        IProductDataService productDataService,
        IDialogService dialogService,
        IMapper mapper,
        IMessenger messenger)
    {
        _navigationService = navigationService;
        _productDataService = productDataService;
        _mapper = mapper;
        _dialogService = dialogService;
        _messenger = messenger;

        State.PropertyChanged += StatePropertyChanged;

        AddCommand = new RelayCommand(NavigateToAddPage);
        EditCommand = new RelayCommand(NavigateToEditPage, () => State.CanEdit);
        DeleteCommand = new AsyncRelayCommand(DeleteSelectedProductAsync, () => State.CanDelete);
        SortCommand = new RelayCommand<(string column, string direction)>(ExecuteSortCommand);
        SearchCommand = new RelayCommand(ExecuteSearchCommand);

        _messenger.RegisterAll(this);
    }
    #endregion

    #region Event Handlers
    /// <summary>
    /// Handles property changes in the ProductState.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments containing the property name.</param>
    private void StatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(State.SelectedProduct))
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
    /// Navigates to the add product page.
    /// </summary>
    private void NavigateToAddPage()
    {
        _navigationService.NavigateTo(typeof(AdminProductAddPageViewModel).FullName);
    }

    /// <summary>
    /// Navigates to the edit product page with the selected product.
    /// </summary>
    private void NavigateToEditPage()
    {
        if (State.SelectedProduct?.ProductId != null)
        {
            _navigationService.NavigateTo(typeof(AdminProductEditPageViewModel).FullName, State.SelectedProduct);
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Loads or reloads the products data grid.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task LoadDataAsync()
    {
        if (State.IsLoading || _isDisposed) return;
        try
        {
            State.IsLoading = true;
            var result = await _productDataService.GetProductsAsync(State.Query);

            var products = _mapper.Map<IEnumerable<ProductViewModel>>(result.Data);
            State.Source = new ObservableCollection<ProductViewModel>(products);
            _messenger.Send(new PaginationChangedMessage(
                result.TotalItems,
                result.CurrentPage,
                result.TotalPages
             ));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.StackTrace);
            await _dialogService.ShowErrorDialog(ex.Message, "Error");
        }
        finally
        {
            State.IsLoading = false;
        }
    }

    /// <summary>
    /// Deletes the selected product after confirmation.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task DeleteSelectedProductAsync()
    {
        if (State.SelectedProduct?.ProductId == null) return;

        try
        {
            var result = await _dialogService.ShowConfirmDeleteDialog();
            if (result != ContentDialogResult.Primary) return;

            var success = await _productDataService.DeleteProductAsync(State.SelectedProduct.ProductId.Value);
            if (success)
            {
                await _dialogService.ShowSuccessDialog("Product deleted successfully.", "Success");
                State.Source.Remove(State.SelectedProduct);
                State.SelectedProduct = null;
                await LoadDataAsync();
            }
            else
            {
                await _dialogService.ShowErrorDialog("Failed to delete product.", "Error");
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
    /// Handles the sorting event of the product data grid.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments containing the column information.</param>
    public void AdminProductDataGrid_Sorting(object sender, DataGridColumnEventArgs e)
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
}