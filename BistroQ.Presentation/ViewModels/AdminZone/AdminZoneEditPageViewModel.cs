using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Models;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.AdminZone;

/// <summary>
/// ViewModel for editing existing zones in the admin interface.
/// Handles form validation, zone updates, and navigation.
/// </summary>
/// <remarks>
/// Implements ObservableRecipient for MVVM pattern and INavigationAware for navigation handling.
/// </remarks>
public partial class AdminZoneEditPageViewModel : ObservableRecipient, INavigationAware
{
    #region Public Properties
    /// <summary>
    /// Gets or sets the zone being edited.
    /// </summary>
    public ZoneViewModel Zone { get; set; }

    /// <summary>
    /// Gets or sets the view model instance.
    /// </summary>
    public AdminZoneEditPageViewModel ViewModel;

    /// <summary>
    /// Gets the command to update the zone.
    /// </summary>
    public ICommand UpdateCommand { get; }
    #endregion

    #region Private Fields
    /// <summary>
    /// Flag indicating whether a zone update operation is in progress.
    /// </summary>
    [ObservableProperty]
    private bool _isProcessing = false;

    /// <summary>
    /// Form model containing zone edit data with validation.
    /// </summary>
    [ObservableProperty]
    private AddZoneForm _form = new();

    /// <summary>
    /// Error message to display when zone update fails.
    /// </summary>
    [ObservableProperty]
    private string _errorMessage = "";

    /// <summary>
    /// Service for managing zone data operations.
    /// </summary>
    private readonly IZoneDataService _zoneDataService;

    /// <summary>
    /// Service for displaying dialog messages to the user.
    /// </summary>
    private readonly IDialogService _dialogService;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the AdminZoneEditPageViewModel class.
    /// </summary>
    /// <param name="zoneDataService">Service for zone data operations.</param>
    /// <param name="dialogService">Service for displaying dialogs.</param>
    public AdminZoneEditPageViewModel(IZoneDataService zoneDataService, IDialogService dialogService)
    {
        _zoneDataService = zoneDataService;
        _dialogService = dialogService;

        UpdateCommand = new AsyncRelayCommand(UpdateZoneAsync);
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
    /// Asynchronously updates the zone after validating the form data.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdateZoneAsync()
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

            var request = new UpdateZoneRequest
            {
                Name = Form.Name
            };

            await _zoneDataService.UpdateZoneAsync(Zone.ZoneId.Value, request);

            await _dialogService.ShowSuccessDialog($"Successfully updated zone: {request.Name}", "Success");
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
    /// Determines whether a zone can be updated based on form validation and processing state.
    /// </summary>
    /// <returns>True if the zone can be updated; otherwise, false.</returns>
    public bool CanUpdate()
    {
        return !Form.HasErrors && !IsProcessing;
    }

    /// <summary>
    /// Handles navigation to this page and initializes the form with the selected zone's data.
    /// </summary>
    /// <param name="parameter">The navigation parameter containing the selected zone.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task OnNavigatedTo(object parameter)
    {
        if (parameter is ZoneViewModel selectedZone)
        {
            Zone = selectedZone;
            Form.Name = Zone?.Name ?? string.Empty;
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