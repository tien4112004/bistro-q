using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using AutoMapper;
using BistroQ.Domain.Contracts.Services.Data;
using BistroQ.Domain.Dtos.Products;
using BistroQ.Presentation.ViewModels.Models;

namespace BistroQ.Presentation.ViewModels.Client;

public partial class ProductListViewModel : ObservableRecipient
{
    private readonly IProductDataService _productService;
    private readonly ICategoryDataService _categoryService;
    private readonly IMapper _mapper;

    [ObservableProperty]
    private bool _isLoadingProduct = false;

    [ObservableProperty]
    private bool _isLoadingCategory = false;

    [ObservableProperty]
    private List<CategoryViewModel> _categories;

    [ObservableProperty]
    private CategoryViewModel _selectedCategory;

    [ObservableProperty]
    private ProductViewModel _selectedProduct;

    [ObservableProperty]
    private ObservableCollection<ProductViewModel> _products = new ObservableCollection<ProductViewModel>();

    public ICommand ChangeCategoryCommand { get; set; }
    public ICommand AddProductToCartCommand { get; set; }

    public ProductListViewModel(
        ICategoryDataService categoryService,
        IProductDataService productService,
        IMapper mapper)
    {
        _categoryService = categoryService;
        _productService = productService;
        _mapper = mapper;
        ChangeCategoryCommand = new AsyncRelayCommand<CategoryViewModel>(ChangeCategory);
    }

    private async Task ChangeCategory(CategoryViewModel category)
    {
        SelectedCategory = category;
        await LoadProductAsync();
        OnPropertyChanged(nameof(Products));
    }

    public async Task LoadCategoriesAsync()
    {
        IsLoadingCategory = true;
        var categoriesData = await _categoryService.GetAllCategoriesAsync();
        var categories = _mapper.Map<IEnumerable<CategoryViewModel>>(categoriesData);
        var allCategory = new CategoryViewModel
        {
            Name = "All",
        };
        Debug.WriteLine(allCategory.CategoryId);
        Categories = new List<CategoryViewModel> { allCategory }.Concat(categories).ToList();
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
            var products = _mapper.Map<IEnumerable<ProductViewModel>>(response.Data);
            Products = new ObservableCollection<ProductViewModel>(products);
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