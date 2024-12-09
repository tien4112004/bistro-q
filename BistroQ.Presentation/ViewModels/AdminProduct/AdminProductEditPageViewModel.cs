using AutoMapper;
using BistroQ.Domain.Contracts.Services.Data;
using BistroQ.Domain.Dtos.Category;
using BistroQ.Domain.Dtos.Products;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Models;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.AdminProduct;

public partial class AdminProductEditPageViewModel : ObservableRecipient, INavigationAware
{
    public ProductViewModel Product { get; set; }
    [ObservableProperty]
    private UpdateProductRequest _request;
    [ObservableProperty]
    private bool _isProcessing = false;
    [ObservableProperty]
    private AddProductForm _form = new();
    [ObservableProperty]
    private string _errorMessage = "";

    public ObservableCollection<CategoryViewModel> Categories;

    private readonly IProductDataService _productDataService;
    private readonly ICategoryDataService _categoryDataService;
    private readonly IDialogService _dialogService;
    private readonly IMapper _mapper;

    public ICommand UpdateCommand { get; }

    public event EventHandler NavigateBack;

    public AdminProductEditPageViewModel(
        IProductDataService productDataService,
        ICategoryDataService categoryDataService,
        IDialogService dialogService,
        IMapper mapper)
    {
        _productDataService = productDataService;
        _categoryDataService = categoryDataService;
        _dialogService = dialogService;
        _mapper = mapper;
        Request = new UpdateProductRequest();
        Categories = new ObservableCollection<CategoryViewModel>();

        UpdateCommand = new AsyncRelayCommand(UpdateProductAsync, CanUpdate);
    }

    public async Task UpdateProductAsync()
    {
        Form.ValidateAll();
        if (!CanUpdate())
        {
            await _dialogService.ShowErrorDialog("Data is invalid. Please check again.", "Error");
            return;
        }

        try
        {
            IsProcessing = true;
            ErrorMessage = string.Empty;

            Request.Name = Form.Name;
            Request.CategoryId = Form.CategoryId;
            Request.Price = Form.Price ?? 0;
            Request.Unit = Form.Unit;
            Request.DiscountPrice = Form.DiscountPrice;

            await _productDataService.UpdateProductAsync(Product.ProductId.Value, Request);

            await _dialogService.ShowSuccessDialog($"Successfully updated product: {Request.Name}", "Success");
            NavigateBack?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await _dialogService.ShowErrorDialog(ex.Message, "Error");
        }
        finally
        {
            IsProcessing = false;
        }
    }

    public bool CanUpdate()
    {
        return !Form.HasErrors && !IsProcessing;
    }

    public async Task LoadCategoriesAsync()
    {
        var response = await _categoryDataService.GetCategoriesAsync(new CategoryCollectionQueryParams
        {
            Size = 1000
        });
        Categories.Clear();
        var categories = _mapper.Map<IEnumerable<CategoryViewModel>>(response.Data);
        foreach (var category in categories)
        {
            Categories.Add(category);
        }
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is ProductViewModel selectedProduct)
        {
            Product = selectedProduct;
            Request.Name = Product?.Name ?? string.Empty;
            Request.Price = Product?.Price ?? 0;
            Request.Unit = Product?.Unit ?? string.Empty;
            Request.DiscountPrice = Product?.DiscountPrice ?? 0;
        }
    }

    public void OnNavigatedFrom()
    {
    }
}