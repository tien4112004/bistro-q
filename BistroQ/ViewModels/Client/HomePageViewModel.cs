using BistroQ.Contracts.Services;
using BistroQ.Contracts.ViewModels;
using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BistroQ.ViewModels.Client;

public partial class HomePageViewModel : ObservableRecipient, INavigationAware
{
    public ProductListViewModel ProductListViewModel { get; }
    public OrderCartViewModel OrderCartViewModel { get; }

    [ObservableProperty]
    private bool _errorMessage;

    public HomePageViewModel(
        IDialogService dialogService,
        IOrderDataService orderDataService,
        ICategoryService categoryService,
        IProductService productService)
    {
        ProductListViewModel = new ProductListViewModel(categoryService, productService);
        OrderCartViewModel = new OrderCartViewModel(orderDataService);

        ProductListViewModel.AddProductToCartRequested += OnAddProductToCartRequested;
        OrderCartViewModel.OrderStartedCommand = new RelayCommand<Order>(OnOrderStarted);
    }

    private void OnAddProductToCartRequested(Product product)
    {
        OrderCartViewModel.AddProductToCart(product);
    }

    private void OnOrderStarted(Order order)
    {
    }

    public async void OnNavigatedTo(object parameter)
    {
        await OrderCartViewModel.LoadExistingOrderAsync();
        _ = ProductListViewModel.LoadCategoriesAsync();
        _ = ProductListViewModel.LoadProductAsync();
    }

    public void OnNavigatedFrom()
    {
        //
    }
}
