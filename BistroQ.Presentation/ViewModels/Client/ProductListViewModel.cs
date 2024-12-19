using AutoMapper;
using BistroQ.Domain.Contracts.Services.Data;
using BistroQ.Domain.Dtos.Products;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

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
    private bool _isEmptyList = false;

    [ObservableProperty]
    private bool _canLoadMore = true;

    private int _page = 1;

    [ObservableProperty]
    private ObservableCollection<ProductViewModel> _products = new ObservableCollection<ProductViewModel>();

    public ICommand ChangeCategoryCommand { get; set; }
    public ICommand LoadMoreProductsCommand { get; set; }

    public ProductListViewModel(
        ICategoryDataService categoryService,
        IProductDataService productService,
        IMapper mapper)
    {
        _categoryService = categoryService;
        _productService = productService;
        _mapper = mapper;
        ChangeCategoryCommand = new AsyncRelayCommand<CategoryViewModel>(ChangeCategory);
        LoadMoreProductsCommand = new AsyncRelayCommand(LoadMoreProductAsync);
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
        var categoriesResponse = await _categoryService.GetCategoriesAsync();
        var categoriesData = categoriesResponse.Data;
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
            _page = 1;
            Products.Clear();
            IsLoadingProduct = true;
            var query = new ProductCollectionQueryParams
            {
                CategoryId = SelectedCategory?.CategoryId ?? null,
                Size = 12,
            };

            var response = await _productService.GetProductsAsync(query);
            var products = _mapper.Map<IEnumerable<ProductViewModel>>(response.Data);
            Products = new ObservableCollection<ProductViewModel>(products);
            CanLoadMore = products.Count() == 12;
            IsEmptyList = !(Products.Any());
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

    public async Task LoadMoreProductAsync()
    {
        try
        {
            _page++;
            IsLoadingProduct = true;
            var query = new ProductCollectionQueryParams
            {
                CategoryId = SelectedCategory?.CategoryId ?? null,
                Size = 12,
                Page = _page
            };

            var response = await _productService.GetProductsAsync(query);
            var products = _mapper.Map<IEnumerable<ProductViewModel>>(response.Data);
            foreach (var product in products)
            {
                Products.Add(product);
            }
            CanLoadMore = products.Count() == 12;
            OnPropertyChanged(nameof(Products));
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