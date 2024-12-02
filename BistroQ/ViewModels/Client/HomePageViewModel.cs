using BistroQ.Contracts.Services;
using BistroQ.Contracts.ViewModels;
using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BistroQ.ViewModels.Client;

public partial class HomePageViewModel : ObservableRecipient, INavigationAware
{
    public ProductListViewModel ProductListViewModel { get; }
    public OrderCartViewModel OrderCartViewModel { get; }

    public ICommand AddProductToCartCommand { get; private set; }

    public HomePageViewModel(
        IDialogService dialogService,
        IOrderDataService orderDataService,
        ICategoryService categoryService,
        IProductService productService)
    {
        ProductListViewModel = App.GetService<ProductListViewModel>();
        OrderCartViewModel = App.GetService<OrderCartViewModel>();

        OrderCartViewModel.OrderStartedCommand = new RelayCommand<Order>(OnOrderStarted);

        AddProductToCartCommand = new RelayCommand<Product>(OnAddProductToCart);
    }

    private void OnAddProductToCart(Product product)
    {
        OrderCartViewModel.AddProductToCart(product);
    }

    private void OnOrderStarted(Order order)
    {
    }

    public async void OnNavigatedTo(object parameter)
    {
        await OrderCartViewModel.LoadExistingOrderAsync();
        await ProductListViewModel.LoadCategoriesAsync();
        await ProductListViewModel.LoadProductAsync();
    }

    public void OnNavigatedFrom()
    {
        //
    }
}
