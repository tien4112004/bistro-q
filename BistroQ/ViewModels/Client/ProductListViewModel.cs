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

    public ICommand ChangeCategoryCommand { get; }
    public ICommand CategoryChangedCommand { get; set; }

    public ProductListViewModel(
        ICategoryService categoryService,
        IProductService productService)
    {
        _categoryService = categoryService;
        _productService = productService;
        ChangeCategoryCommand = new AsyncRelayCommand(ChangeCategory);
    }

    private async Task ChangeCategory()
    {
        await LoadProductAsync();
    }

    public async Task LoadCategoriesAsync()
    {
        IsLoadingCategory = true;
        var categories = await _categoryService.GetAllCategoriesAsync();
        var allCategory = new Category
        {
            Name = "All"
        };
        await Task.Delay(1000); // For test the loading animation
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
            await Task.Delay(1500); // For test the loading animation
            Products = new ObservableCollection<Product>(response.Data);
            CategoryChangedCommand?.Execute(SelectedCategory);
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
