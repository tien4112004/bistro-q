using AutoMapper;
using BistroQ.Domain.Contracts.Services.Data;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Products;
using BistroQ.Domain.Models.Entities;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.Client;

/// <summary>
/// ViewModel responsible for managing the product list functionality in the client interface.
/// Handles product loading, category filtering, and pagination support.
/// </summary>
/// <remarks>
/// Implements ObservableRecipient for MVVM pattern support and change notifications.
/// </remarks>
public partial class ProductListViewModel : ObservableRecipient
{
    #region Private Fields
    /// <summary>
    /// Service for handling product data operations
    /// </summary>
    private readonly IProductDataService _productService;

    /// <summary>
    /// Service for handling category data operations
    /// </summary>
    private readonly ICategoryDataService _categoryService;

    /// <summary>
    /// AutoMapper instance for object mapping
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Current page number for pagination
    /// </summary>
    private int _page = 1;
    #endregion

    #region Observable Properties
    /// <summary>
    /// Gets or sets whether products are currently being loaded
    /// </summary>
    [ObservableProperty]
    private bool _isLoadingProduct = false;

    /// <summary>
    /// Gets or sets whether categories are currently being loaded
    /// </summary>
    [ObservableProperty]
    private bool _isLoadingCategory = false;

    /// <summary>
    /// Gets or sets the list of available categories
    /// </summary>
    [ObservableProperty]
    private List<CategoryViewModel> _categories;

    /// <summary>
    /// Gets or sets the currently selected category
    /// </summary>
    [ObservableProperty]
    private CategoryViewModel _selectedCategory;

    /// <summary>
    /// Gets or sets the currently selected product
    /// </summary>
    [ObservableProperty]
    private ProductViewModel _selectedProduct;

    /// <summary>
    /// Gets or sets whether the product list is empty
    /// </summary>
    [ObservableProperty]
    private bool _isEmptyList = false;

    /// <summary>
    /// Gets or sets whether more products can be loaded
    /// </summary>
    [ObservableProperty]
    private bool _canLoadMore = true;

    /// <summary>
    /// Gets or sets the collection of products currently displayed
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<ProductViewModel> _products = new ObservableCollection<ProductViewModel>();
    #endregion

    #region Commands
    /// <summary>
    /// Command to handle category change operations
    /// </summary>
    public ICommand ChangeCategoryCommand { get; set; }

    /// <summary>
    /// Command to load more products for pagination
    /// </summary>
    public ICommand LoadMoreProductsCommand { get; set; }
    #endregion

    #region Constructor
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
    #endregion

    #region Private Methods
    /// <summary>
    /// Changes the current category and reloads the product list
    /// </summary>
    /// <param name="category">The category to switch to</param>
    /// <returns>A task representing the asynchronous operation</returns>
    private async Task ChangeCategory(CategoryViewModel category)
    {
        SelectedCategory = category;
        await LoadProductsAsync();
        OnPropertyChanged(nameof(Products));
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Loads all available categories including special categories (All, Recommended)
    /// </summary>
    /// <returns>A task representing the asynchronous operation</returns>
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
        var recommendedCategory = new CategoryViewModel
        {
            Name = "Recommended",
        };

        Categories = new List<CategoryViewModel> { recommendedCategory, allCategory }.Concat(categories).ToList();
        IsLoadingCategory = false;
    }

    /// <summary>
    /// Loads products based on the selected category
    /// </summary>
    /// <remarks>
    /// If "Recommended" category is selected, loads recommended products.
    /// Otherwise, loads products from the selected category with pagination.
    /// </remarks>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task LoadProductsAsync()
    {
        try
        {
            _page = 1;
            Products.Clear();
            IsLoadingProduct = true;

            ApiCollectionResponse<IEnumerable<Product>> response;

            if (SelectedCategory?.Name == "Recommended")
            {
                response = await _productService.GetRecommendationAsync();
            }
            else
            {
                var query = new ProductCollectionQueryParams
                {
                    CategoryId = SelectedCategory?.CategoryId ?? null,
                    Size = 12,
                };
                response = await _productService.GetProductsAsync(query);
            }

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

    /// <summary>
    /// Loads the next page of products for the current category
    /// </summary>
    /// <returns>A task representing the asynchronous operation</returns>
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
    #endregion
}