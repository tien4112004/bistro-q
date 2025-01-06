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

/// <summary>
/// ViewModel for editing existing categories in the admin interface.
/// Manages the category editing form and handles the update operation.
/// </summary>
/// <remarks>
/// Implements INavigationAware for handling navigation events and uses MVVM pattern
/// with ObservableRecipient as its base class.
/// </remarks>
public partial class AdminCategoryEditPageViewModel : ObservableRecipient, INavigationAware
{
    #region Private Fields
    private readonly ICategoryDataService _categoryDataService;
    private readonly IDialogService _dialogService;
    #endregion

    #region Public Properties
    /// <summary>
    /// The category being edited.
    /// </summary>
    public CategoryViewModel Category { get; set; }
    #endregion

    #region Observable Properties
    /// <summary>
    /// Indicates whether the ViewModel is currently processing a request.
    /// </summary>
    [ObservableProperty]
    private bool _isProcessing = false;

    /// <summary>
    /// The form containing category editing data.
    /// </summary>
    [ObservableProperty]
    private AddCategoryForm _form = new();

    /// <summary>
    /// Error message to display when operations fail.
    /// </summary>
    [ObservableProperty]
    private string _errorMessage = "";
    #endregion

    #region Commands
    /// <summary>
    /// Command for updating the category.
    /// </summary>
    public ICommand UpdateCommand { get; }
    #endregion

    #region Constructor
    public AdminCategoryEditPageViewModel(ICategoryDataService categoryDataService, IDialogService dialogService)
    {
        _categoryDataService = categoryDataService;
        _dialogService = dialogService;

        UpdateCommand = new AsyncRelayCommand(async () => await UpdateCategoryAsync(), CanUpdate);
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
    /// Updates the existing category using the form data.
    /// Validates the form, updates the category, and handles any errors that occur.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Determines whether the category can be updated based on form validation and processing state.
    /// </summary>
    /// <returns>True if the category can be updated; otherwise, false.</returns>
    public bool CanUpdate()
    {
        return !Form.HasErrors && !IsProcessing;
    }

    /// <summary>
    /// Handles navigation to this page, initializing the form with the selected category's data.
    /// </summary>
    /// <param name="parameter">Navigation parameter containing the category to edit.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task OnNavigatedTo(object parameter)
    {
        if (parameter is CategoryViewModel selectedCategory)
        {
            Category = selectedCategory;
            Form.Name = Category?.Name ?? string.Empty;
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