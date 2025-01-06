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

/// <summary>
/// ViewModel for editing existing products in the admin interface.
/// Manages the product editing form, category selection, and image upload functionality.
/// </summary>
/// <remarks>
/// Implements INavigationAware for handling navigation events and uses MVVM pattern
/// with ObservableRecipient as its base class.
/// </remarks>
public partial class AdminProductEditPageViewModel : ObservableRecipient, INavigationAware
{
    #region Private Fields
    private readonly IProductDataService _productDataService;
    private readonly ICategoryDataService _categoryDataService;
    private readonly IDialogService _dialogService;
    private readonly IMapper _mapper;
    #endregion

    #region Observable Properties
    /// <summary>
    /// Indicates whether the ViewModel is currently processing a request.
    /// </summary>
    [ObservableProperty]
    private bool _isProcessing = false;

    /// <summary>
    /// The form containing product editing data including product details and image.
    /// </summary>
    [ObservableProperty]
    private AddProductForm _form = new();

    /// <summary>
    /// Error message to display when operations fail.
    /// </summary>
    [ObservableProperty]
    private string _errorMessage = "";
    #endregion

    #region Public Properties
    /// <summary>
    /// Collection of available categories for product assignment.
    /// </summary>
    public ObservableCollection<CategoryViewModel> Categories;
    #endregion

    #region Commands
    /// <summary>
    /// Command for updating the product details.
    /// </summary>
    public ICommand UpdateCommand { get; }
    #endregion

    #region Constructor
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
    #endregion

    #region Public Methods
    /// <summary>
    /// Navigates back to the previous page.
    /// </summary>
    public void NavigateBack()
    {
        App.GetService<INavigationService>().GoBack();
    }

    /// <summary>
    /// Updates the product using the form data.
    /// Validates the form, updates the product details and image if provided, and handles any errors.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
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
                Calories = (decimal)Form.Calories,
                Fat = (decimal)Form.Fat,
                Fiber = (decimal)Form.Fiber,
                Protein = (decimal)Form.Protein,
                Carbohydrates = (decimal)Form.Carbohydrates,
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

    /// <summary>
    /// Determines whether the product can be updated based on form validation and processing state.
    /// </summary>
    /// <returns>True if the product can be updated; otherwise, false.</returns>
    public bool CanUpdate()
    {
        return !Form.HasErrors && !IsProcessing;
    }

    /// <summary>
    /// Loads the list of available categories for product assignment.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Handles navigation to this page, initializing the form with the selected product's data.
    /// </summary>
    /// <param name="parameter">Navigation parameter containing the product to edit.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Handles navigation from this page.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task OnNavigatedFrom()
    {
        return Task.CompletedTask;
    }
    #endregion
}