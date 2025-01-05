using AutoMapper;
using BistroQ.Domain.Contracts.Services.Data;
using BistroQ.Domain.Dtos.Category;
using BistroQ.Domain.Dtos.Products;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Models;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BistroQ.Presentation.ViewModels.AdminProduct;

/// <summary>
/// ViewModel for adding new products in the admin interface.
/// Manages the product creation form, category selection, and image upload.
/// </summary>
/// <remarks>
/// Uses MVVM pattern with ObservableRecipient as its base class for property change notifications
/// and command handling.
/// </remarks>
public partial class AdminProductAddPageViewModel : ObservableRecipient
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
    /// The form containing product creation data including product details and image.
    /// </summary>
    [ObservableProperty]
    private AddProductForm _form = new();

    /// <summary>
    /// Error message to display when operations fail.
    /// </summary>
    [ObservableProperty]
    private string _errorMessage = string.Empty;
    #endregion

    #region Public Properties
    /// <summary>
    /// Collection of available categories for product assignment.
    /// </summary>
    public ObservableCollection<CategoryViewModel> Categories;
    #endregion

    #region Commands
    /// <summary>
    /// Command for adding a new product.
    /// </summary>
    public IRelayCommand AddCommand { get; }
    #endregion

    #region Event Handlers
    /// <summary>
    /// Handles changes to the form property.
    /// Updates the AddCommand's execution state based on form validity.
    /// </summary>
    /// <param name="value">The new form value.</param>
    partial void OnFormChanged(AddProductForm value)
    {
        AddCommand.NotifyCanExecuteChanged();
    }
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the AdminProductAddPageViewModel class.
    /// </summary>
    /// <param name="productDataService">Service for product-related operations.</param>
    /// <param name="categoryDataService">Service for category-related operations.</param>
    /// <param name="dialogService">Service for showing dialogs.</param>
    /// <param name="mapper">AutoMapper instance for object mapping.</param>
    public AdminProductAddPageViewModel(
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
        LoadCategoriesAsync();

        AddCommand = new AsyncRelayCommand(AddProductAsync);
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
    /// Adds a new product using the form data.
    /// Validates the form, creates the product with image if provided, and handles any errors.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddProductAsync()
    {
        Form.ValidateAll();
        if (Form.HasErrors)
        {
            await _dialogService.ShowErrorDialog("Data is invalid. Please check again.", "Error");
            return;
        }

        try
        {
            IsProcessing = true;
            ErrorMessage = string.Empty;

            var request = new CreateProductRequest
            {
                Name = Form.Name,
                CategoryId = Form.CategoryId,
                Price = Form.Price,
                Unit = Form.Unit,
                DiscountPrice = Form.DiscountPrice,
                Calories = Form.Calories,
                Fat = Form.Fat,
                Fiber = Form.Fiber,
                Protein = Form.Protein,
                Carbohydrates = Form.Carbohydrates,
            };

            await _productDataService.CreateProductAsync(request,
                Form.ImageFile?.Stream, Form.ImageFile?.FileName, Form.ImageFile?.ContentType);

            await _dialogService.ShowSuccessDialog("Successfully added product: " + request.Name, "Success");
            NavigateBack();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.StackTrace);
            ErrorMessage = ex.Message;
            await _dialogService.ShowErrorDialog(ex.Message, "Error");
        }
        finally
        {
            IsProcessing = false;
        }
    }

    /// <summary>
    /// Loads the list of available categories for product assignment.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task LoadCategoriesAsync()
    {
        try
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
        catch (Exception ex)
        {
            Categories = new ObservableCollection<CategoryViewModel>();
            await _dialogService.ShowErrorDialog(ex.Message, "Error");
        }
    }
    #endregion
}