using BistroQ.Contracts.Services;
using BistroQ.Contracts.ViewModels;
using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos.Products;
using BistroQ.Core.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Windows.Input;

namespace BistroQ.ViewModels.Client;

public partial class HomePageViewModel : ObservableRecipient, INavigationAware
{
    private readonly IOrderDataService _orderDataService;
    private readonly ICategoryService _categoryService;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private readonly IProductService _productService;

    public Order Order { get; set; }

    [ObservableProperty]
    private bool _isOrdering;

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    /// First VM

    [ObservableProperty]
    private bool _categoryIsLoading = false;

    [ObservableProperty]
    private List<Category> _categories;

    [ObservableProperty]
    private Category _selectedCategory;

    [ObservableProperty]
    private List<Product> _products = new List<Product>
    {
        new Product
        {
            CategoryId = 2,
            Name = "Sản phẩm 2",
            Price=200000,
            Unit="Test"
        },
        new Product
        {
            CategoryId = 2,
            Name = "Long product bun bo hue pho bo tra da",
            Price=999999999,
            Unit="Test"
        },
        new Product
        {
            CategoryId = 2,
            Name = "Tra da",
            Price=888888888888,
            Unit="Test"
        }
    };

    // Second VM
    [ObservableProperty]
    private List<OrderDetail> _orderDetails = new List<OrderDetail>
        {
            new OrderDetail
            {
                OrderDetailId = "1",
                OrderId = null,
                ProductId = 1,
                Quantity = 1,
                PriceAtPurchase = 1,
                Product = new Product
                {
                    ProductId = 1,
                    Name = "A long",
                }
            },
            new OrderDetail
            {
                OrderDetailId = "1",
                OrderId = null,
                ProductId = 1,
                Quantity = 1,
                PriceAtPurchase = 1,
                Product = new Product
                {
                    ProductId = 1,
                    Name = "A very long description A very long description A very long description A very long description",
                }
            },
            new OrderDetail
            {
                OrderDetailId = "1",
                OrderId = null,
                ProductId = 1,
                Quantity = 1,
                PriceAtPurchase = 1,
                Product = new Product
                {
                    ProductId = 1,
                    Name = "A long",
                }
            },
            new OrderDetail
            {
                OrderDetailId = "1",
                OrderId = null,
                ProductId = 1,
                Quantity = 1,
                PriceAtPurchase = 1,
                Product = new Product
                {
                    ProductId = 1,
                    Name = "A very long description A very long description A very long description A very long description",
                }
            },
        };



    public ICommand StartOrderCommand { get; }
    public ICommand CancelOrderCommand { get; }
    public ICommand ChangeCategoryCommand { get; }
    public ICommand ShowProductDetailCommand { get; }
    public HomePageViewModel(
        IDialogService dialogService,
        IOrderDataService orderDataService,
        ICategoryService categoryService,
        IProductService productService)
    {
        _dialogService = dialogService;
        _orderDataService = orderDataService;
        _categoryService = categoryService;
        _productService = productService;
        StartOrderCommand = new AsyncRelayCommand(StartOrder);
        CancelOrderCommand = new RelayCommand(CancelOrder);
        ChangeCategoryCommand = new AsyncRelayCommand(ChangeCategory);
        //ShowProductDetailCommand = new RelayCommand<Product>(ShowProductDetailCommand);
    }



    //public event OrderNewItem { get; set; }
    private async Task ChangeCategory()
    {
        try
        {
            _isLoading = true;

            var query = new ProductCollectionQueryParams
            {
                CategoryId = _selectedCategory?.CategoryId ?? null
            };

            var response = await _productService.GetProductsAsync(query);

            Products = response.Data.ToList();

            _isLoading = false;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        finally
        {
            _isLoading = false;
        }

    }

    private async Task StartOrder()
    {
        try
        {
            IsLoading = true;
            var categories = await _categoryService.GetAllCategoriesAsync();

            await Task.Delay(400); // TODO: This if for UI visualize only, remove it afterward  

            Order = await Task.Run(
                async () =>
                {
                    return await _orderDataService.CreateOrderAsync();
                });
            IsOrdering = true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void CancelOrder()
    {
        _orderDataService.DeleteOrderAsync();
        Order = null;

        ErrorMessage = string.Empty;
        IsOrdering = false;
    }

    public async void OnNavigatedTo(object parameter)
    {
        IsLoading = true;
        var existingOrder = await Task.Run(async () =>
        {
            return await _orderDataService.GetOrderAsync();

        });

        _categoryIsLoading = true;
        var categories = await _categoryService.GetAllCategoriesAsync();
        var allCategory = new Category
        {
            Name = "All"
        };
        Categories = new List<Category> { allCategory }.Concat(categories)
                                                       .ToList();
        _categoryIsLoading = false;

        IsLoading = false;

        if (existingOrder == null)
        {
            return;
        }
        Order = existingOrder;
        IsOrdering = true;

    }

    public void OnNavigatedFrom()
    {
        //
    }
}
