﻿using BistroQ.Contracts.Services;
using BistroQ.Contracts.ViewModels;
using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BistroQ.ViewModels.Client;

public partial class HomePageViewModel : ObservableRecipient, INavigationAware
{
    private readonly IOrderDataService _orderDataService;
    private readonly ICategoryService _categoryService;

    public event EventHandler<Order> OrderCreated;
    public event EventHandler OrderCancelled;
    public event EventHandler<IEnumerable<Category>> CategoriesLoaded;
    public event EventHandler<string> ErrorOccurred;

    public Order Order { get; set; }

    [ObservableProperty]
    private bool _isOrdering;

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    private bool _categoryIsLoading = false;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private List<Category> _categories;

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

    [ObservableProperty]
    private List<Product> _products = new List<Product>
    {
        new Product
        {
            CategoryId=1,
            Name = "San phamar 1",
            Price=100,
            Unit="Test"
        },
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

    public ICommand StartOrderCommand { get; }
    public ICommand CancelOrderCommand { get; }
    public HomePageViewModel(IOrderDataService orderDataService, ICategoryService categoryService)
    {
        _orderDataService = orderDataService;
        _categoryService = categoryService;
        StartOrderCommand = new AsyncRelayCommand(StartOrder);
        CancelOrderCommand = new RelayCommand(CancelOrder);
    }

    //public event OrderNewItem { get; set; }

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
        System.Threading.Thread.Sleep(3000);
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
