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
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels;

public partial class AdminProductViewModel :
    ObservableRecipient,
    INavigationAware,
    IDisposable
{
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private readonly IProductDataService _productDataService;
    private readonly IMapper _mapper;
    private readonly IMessenger _messenger;

    [ObservableProperty]
    private AdminProductState state = new();

    public IRelayCommand AddCommand { get; }
    public IRelayCommand EditCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }
    public ICommand SortCommand { get; }
    public ICommand SearchCommand { get; }

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

        RegisterMessengers();
    }

    private void RegisterMessengers()
    {
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
        if (e.PropertyName == nameof(State.SelectedProduct))
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
        State.SelectedProduct = null;
    }

    private async Task LoadDataAsync()
    {
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
            await _dialogService.ShowErrorDialog(ex.Message, "Error");
        }
        finally
        {
            State.IsLoading = false;
        }
    }

    private void NavigateToAddPage() =>
        _navigationService.NavigateTo(typeof(AdminProductAddPageViewModel).FullName);

    private void NavigateToEditPage()
    {
        if (State.SelectedProduct?.ProductId != null)
        {
            _navigationService.NavigateTo(typeof(AdminProductEditPageViewModel).FullName, State.SelectedProduct);
        }
    }

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

    private void ExecuteSortCommand((string column, string direction) sortParams)
    {
        var (column, direction) = sortParams;
        State.Query.OrderBy = column;
        State.Query.OrderDirection = direction;
        State.ReturnToFirstPage();
        _ = LoadDataAsync();
    }

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

        _messenger.UnregisterAll(this);
    }
}