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

public partial class AdminZoneEditPageViewModel : ObservableRecipient, INavigationAware
{
    public ZoneViewModel Zone { get; set; }
    [ObservableProperty]
    private bool _isProcessing = false;
    [ObservableProperty]
    private AddZoneForm _form = new();
    [ObservableProperty]
    private string _errorMessage = "";

    public AdminZoneEditPageViewModel ViewModel;

    private readonly IZoneDataService _zoneDataService;
    private readonly IDialogService _dialogService;

    public ICommand UpdateCommand { get; }

    public event EventHandler NavigateBack;
    public AdminZoneEditPageViewModel(IZoneDataService zoneDataService)
    {
        _zoneDataService = zoneDataService;
        _zoneDataService = zoneDataService;

        UpdateCommand = new AsyncRelayCommand(UpdateZoneAsync);
    }

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
            NavigateBack?.Invoke(this, EventArgs.Empty);
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

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is ZoneViewModel selectedZone)
        {
            Zone = selectedZone;
            Form.Name = Zone?.Name ?? string.Empty;
        }
    }

    public bool CanUpdate()
    {
        return !Form.HasErrors && !IsProcessing;
    }

    public void OnNavigatedFrom()
    {
    }
}