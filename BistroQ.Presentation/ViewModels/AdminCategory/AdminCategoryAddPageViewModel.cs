using BistroQ.Domain.Contracts.Services.Data;
using BistroQ.Domain.Dtos.Category;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.AdminCategory;

public partial class AdminCategoryAddPageViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool _isProcessing = false;
    [ObservableProperty]
    private AddCategoryForm _form = new();
    [ObservableProperty]
    private string _errorMessage = string.Empty;

    private readonly ICategoryDataService _categoryDataService;
    private readonly IDialogService _dialogService;

    public ICommand AddCommand { get; }

    public AdminCategoryAddPageViewModel(ICategoryDataService categoryDataService, IDialogService dialogService)
    {
        _categoryDataService = categoryDataService;
        _dialogService = dialogService;

        AddCommand = new AsyncRelayCommand(AddCategoryAsync);
    }

    public void NavigateBack()
    {
        App.GetService<INavigationService>().GoBack();
    }

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
}