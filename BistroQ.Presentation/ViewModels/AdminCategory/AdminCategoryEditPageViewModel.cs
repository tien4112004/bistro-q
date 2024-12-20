using BistroQ.Domain.Contracts.Services.Data;
using BistroQ.Domain.Dtos.Category;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Models;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.AdminCategory;

public partial class AdminCategoryEditPageViewModel : ObservableRecipient, INavigationAware
{
    public CategoryViewModel Category { get; set; }
    [ObservableProperty]
    private bool _isProcessing = false;
    [ObservableProperty]
    private AddCategoryForm _form = new();
    [ObservableProperty]
    private string _errorMessage = "";

    private readonly ICategoryDataService _categoryDataService;
    private readonly IDialogService _dialogService;

    public ICommand UpdateCommand { get; }

    public AdminCategoryEditPageViewModel(ICategoryDataService categoryDataService, IDialogService dialogService)
    {
        _categoryDataService = categoryDataService;
        _dialogService = dialogService;

        UpdateCommand = new AsyncRelayCommand(async () => await UpdateCategoryAsync(), CanUpdate);
    }

    public void NavigateBack()
    {
        App.GetService<INavigationService>().GoBack();
    }

    public async Task UpdateCategoryAsync()
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

            var request = new UpdateCategoryRequest
            {
                Name = Form.Name
            };

            await _categoryDataService.UpdateCategoryAsync(Category.CategoryId.Value, request);

            await _dialogService.ShowSuccessDialog($"Successfully updated category: {request.Name}", "Success");
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

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is CategoryViewModel selectedCategory)
        {
            Category = selectedCategory;
            Form.Name = Category?.Name ?? string.Empty;
        }
    }

    public void OnNavigatedFrom()
    {
    }
}