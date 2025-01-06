using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.AdminZone;

/// <summary>
/// ViewModel for adding new zones in the admin interface.
/// Handles form validation, zone creation, and navigation.
/// </summary>
/// <remarks>
/// Inherits from ObservableRecipient for MVVM pattern implementation.
/// </remarks>
public partial class AdminZoneAddPageViewModel : ObservableRecipient
{
    #region Private Fields
    /// <summary>
    /// Flag indicating whether a zone creation operation is in progress.
    /// </summary>
    [ObservableProperty]
    private bool _isProcessing = false;

    /// <summary>
    /// Form model containing zone creation data with validation.
    /// </summary>
    [ObservableProperty]
    private AddZoneForm _form = new();

    /// <summary>
    /// Error message to display when zone creation fails.
    /// </summary>
    [ObservableProperty]
    private string _errorMessage = string.Empty;

    /// <summary>
    /// Service for managing zone data operations.
    /// </summary>
    private readonly IZoneDataService _zoneDataService;

    /// <summary>
    /// Service for displaying dialog messages to the user.
    /// </summary>
    private readonly IDialogService _dialogService;
    #endregion

    #region Public Properties
    /// <summary>
    /// Command to trigger the zone creation process.
    /// </summary>
    public ICommand AddCommand { get; }
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the AdminZoneAddPageViewModel class.
    /// </summary>
    /// <param name="zoneDataService">Service for zone data operations.</param>
    /// <param name="dialogService">Service for displaying dialogs.</param>
    public AdminZoneAddPageViewModel(IZoneDataService zoneDataService, IDialogService dialogService)
    {
        _zoneDataService = zoneDataService;
        _dialogService = dialogService;
        AddCommand = new AsyncRelayCommand(AddZoneAsync, CanAdd);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Determines whether a zone can be added based on form validation and processing state.
    /// </summary>
    /// <returns>True if the zone can be added; otherwise, false.</returns>
    public bool CanAdd()
    {
        return !Form.HasErrors && !IsProcessing;
    }

    /// <summary>
    /// Navigates back to the previous page.
    /// </summary>
    public void NavigateBack()
    {
        App.GetService<INavigationService>().GoBack();
    }

    /// <summary>
    /// Asynchronously adds a new zone after validating the form data.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task AddZoneAsync()
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
            var request = new CreateZoneRequest
            {
                Name = Form.Name
            };
            await _zoneDataService.CreateZoneAsync(request);
            await _dialogService.ShowErrorDialog("Successfully added zone: " + request.Name, "Success");
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