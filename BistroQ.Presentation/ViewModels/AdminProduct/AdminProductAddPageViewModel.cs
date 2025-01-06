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

public partial class AdminProductAddPageViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool _isProcessing = false;
    [ObservableProperty]
    private AddProductForm _form = new();
    [ObservableProperty]
    private string _errorMessage = string.Empty;

    partial void OnFormChanged(AddProductForm value)
    {
        AddCommand.NotifyCanExecuteChanged();
    }

    public ObservableCollection<CategoryViewModel> Categories;

    private readonly IProductDataService _productDataService;
    private readonly ICategoryDataService _categoryDataService;
    private readonly IDialogService _dialogService;
    private readonly IMapper _mapper;

    public IRelayCommand AddCommand { get; }

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

    public void NavigateBack()
    {
        App.GetService<INavigationService>().GoBack();
    }

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
                Calories = (decimal)Form.Calories,
                Fat = (decimal)Form.Fat,
                Fiber = (decimal)Form.Fiber,
                Protein = (decimal)Form.Protein,
                Carbohydrates = (decimal)Form.Carbohydrates,
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
}