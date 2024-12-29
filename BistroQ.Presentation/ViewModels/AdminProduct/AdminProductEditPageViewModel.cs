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
        Categories = new ObservableCollection<CategoryViewModel>();

        UpdateCommand = new AsyncRelayCommand(UpdateProductAsync, CanUpdate);
    }

    public void NavigateBack()
    {
        App.GetService<INavigationService>().GoBack();
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
                CategoryId = Form.CategoryId,
                Calories = Form.Calories,
                Fat = Form.Fat,
                Fiber = Form.Fiber,
                Protein = Form.Protein,
                Carbohydrates = Form.Carbohydrates,
            };

            if (Form.ImageFile != null)
            {
                await _productDataService.UpdateProductImageAsync(Form.ProductId.Value, Form.ImageFile.Stream, Form.ImageFile.FileName, Form.ImageFile.ContentType);
            }
            await _productDataService.UpdateProductAsync(Form.ProductId.Value, request);

            await _dialogService.ShowSuccessDialog($"Successfully updated product: {request.Name}", "Success");
            NavigateBack();
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

    public Task OnNavigatedTo(object parameter)
    {
        if (parameter is ProductViewModel selectedProduct)
        {
            Form.ProductId = selectedProduct.ProductId;
            Form.Name = selectedProduct?.Name ?? string.Empty;
            Form.Price = selectedProduct?.Price ?? 0;
            Form.Unit = selectedProduct?.Unit ?? string.Empty;
            Form.DiscountPrice = selectedProduct?.DiscountPrice ?? 0;
            Form.ImageUrl = selectedProduct?.ImageUrl;
            Form.CategoryId = selectedProduct?.Category?.CategoryId ?? 0;
            Form.Calories = double.TryParse(selectedProduct?.NutritionFact?.Calories, out var calories) ? calories : 0.0;
            Form.Fat = double.TryParse(selectedProduct?.NutritionFact?.Fat, out var fat) ? fat : 0.0;
            Form.Fiber = double.TryParse(selectedProduct?.NutritionFact?.Fiber, out var fiber) ? fiber : 0.0;
            Form.Protein = double.TryParse(selectedProduct?.NutritionFact?.Protein, out var protein) ? protein : 0.0;
            Form.Carbohydrates = double.TryParse(selectedProduct?.NutritionFact?.Carbohydrates, out var carbohydrates) ? carbohydrates : 0.0;
        }

        return Task.CompletedTask;
    }

    public Task OnNavigatedFrom()
    {
        return Task.CompletedTask;
    }
}