using BistroQ.Domain.Contracts.Services.Data;
using BistroQ.Domain.Dtos.Category;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.AdminCategory;

/// <summary>
/// ViewModel for adding new categories in the admin interface.
/// Manages the category creation form and handles the add operation.
/// </summary>
/// <remarks>
/// Uses MVVM pattern with ObservableRecipient as its base class for property change notifications
/// and command handling.
/// </remarks>
public partial class AdminCategoryAddPageViewModel : ObservableRecipient
{
    #region Private Fields
    private readonly ICategoryDataService _categoryDataService;
    private readonly IDialogService _dialogService;
    #endregion

    #region Observable Properties
    /// <summary>
    /// Indicates whether the ViewModel is currently processing a request.
    /// </summary>
    [ObservableProperty]
    private bool _isProcessing = false;

    /// <summary>
    /// The form containing category creation data.
    /// </summary>
    [ObservableProperty]
    private AddCategoryForm _form = new();

    /// <summary>
    /// Error message to display when operations fail.
    /// </summary>
    [ObservableProperty]
    private string _errorMessage = string.Empty;
    #endregion

    #region Commands
    /// <summary>
    /// Command for adding a new category.
    /// </summary>
    public ICommand AddCommand { get; }
    #endregion

    #region Constructor
    public AdminCategoryAddPageViewModel(ICategoryDataService categoryDataService, IDialogService dialogService)
    {
        _categoryDataService = categoryDataService;
        _dialogService = dialogService;
        AddCommand = new AsyncRelayCommand(AddCategoryAsync);
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
    /// Adds a new category using the form data.
    /// Validates the form, creates the category, and handles any errors that occur.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddCategoryAsync()
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

            var request = new CreateCategoryRequest
            {
                Name = Form.Name
            };

            await _categoryDataService.CreateCategoryAsync(request);
            await _dialogService.ShowErrorDialog("Successfully added category: " + request.Name, "Success");
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
    #endregion
}