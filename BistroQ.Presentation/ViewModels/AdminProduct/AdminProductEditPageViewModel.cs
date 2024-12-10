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
using System.Diagnostics;
using System.Text.Json;
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

            var request = new UpdateProductRequest
            {
                Name = Form.Name,
                Price = Form.Price,
                Unit = Form.Unit,
                DiscountPrice = Form.DiscountPrice,
                CategoryId = Form.CategoryId
            };

            await _productDataService.UpdateProductAsync(Product.ProductId.Value, request);

            await _dialogService.ShowSuccessDialog($"Successfully updated product: {request.Name}", "Success");
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
            Form.Name = Product?.Name ?? string.Empty;
            Form.Price = Product?.Price ?? 0;
            Form.Unit = Product?.Unit ?? string.Empty;
            Form.DiscountPrice = Product?.DiscountPrice ?? 0;
            Form.CategoryId = Product?.Category?.CategoryId ?? 0;
            Debug.WriteLine(JsonSerializer.Serialize(Form));
        }
    }

    public void OnNavigatedFrom()
    {
    }
}