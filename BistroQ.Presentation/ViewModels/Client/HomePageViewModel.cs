using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Contracts.Services.Data;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.Client;

public partial class HomePageViewModel : ObservableRecipient, INavigationAware
{
    public ProductListViewModel ProductListViewModel { get; }
    public OrderCartViewModel OrderCartViewModel { get; }

    public ICommand AddProductToCartCommand { get; private set; }

    public HomePageViewModel(
        IDialogService dialogService,
        IOrderDataService orderDataService,
        ICategoryDataService categoryService,
        IProductDataService productService)
    {
        ProductListViewModel = App.GetService<ProductListViewModel>();
        OrderCartViewModel = App.GetService<OrderCartViewModel>();

        OrderCartViewModel.OrderStartedCommand = new RelayCommand<OrderViewModel>(OnOrderStarted);
    }

    private void OnOrderStarted(OrderViewModel order)
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
        OrderCartViewModel.Dispose();
    }
}