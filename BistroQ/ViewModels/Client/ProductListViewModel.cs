using BistroQ.Contracts.Services;
using BistroQ.Core.Dtos.Products;
using BistroQ.Core.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace BistroQ.ViewModels.Client;

public partial class ProductListViewModel : ObservableRecipient
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    [ObservableProperty]
    private bool _isLoadingProduct = false;

    [ObservableProperty]
    private bool _isLoadingCategory = false;

    [ObservableProperty]
    private List<Category> _categories;

    [ObservableProperty]
    private Category _selectedCategory;

    [ObservableProperty]
    private ObservableCollection<Product> _products = new ObservableCollection<Product>();

    public ICommand ChangeCategoryCommand { get; set; }
    public ICommand AddProductToCartCommand { get; set; }

    public event Action<Product> AddProductToCartRequested;

    public ProductListViewModel(
        ICategoryService categoryService,
        IProductService productService)
    {
        _categoryService = categoryService;
        _productService = productService;
        ChangeCategoryCommand = new AsyncRelayCommand<Category>(ChangeCategory);
        AddProductToCartCommand = new RelayCommand<Product>(AddProductToCart);
    }

    private async Task ChangeCategory(Category category)
    {
        SelectedCategory = category;
        await LoadProductAsync();
        OnPropertyChanged(nameof(Products));
    }

    private void AddProductToCart(Product product)
    {
        AddProductToCartRequested?.Invoke(product);
    }

    public async Task LoadCategoriesAsync()
    {
        IsLoadingCategory = true;
        var categories = await _categoryService.GetAllCategoriesAsync();
        var allCategory = new Category
        {
            Name = "All"
        };
        await Task.Delay(400); // For test the loading animation
        Categories = new List<Category> { allCategory }.Concat(categories).ToList();
        IsLoadingCategory = false;
    }

    public async Task LoadProductAsync()
    {
        try
        {
            Products.Clear();
            IsLoadingProduct = true;

            var query = new ProductCollectionQueryParams
            {
                CategoryId = SelectedCategory?.CategoryId ?? null
            };

            var response = await _productService.GetProductsAsync(query);
            await Task.Delay(800); // For test the loading animation
            Products = new ObservableCollection<Product>(response.Data);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        finally
        {
            IsLoadingProduct = false;
        }
    }
}
