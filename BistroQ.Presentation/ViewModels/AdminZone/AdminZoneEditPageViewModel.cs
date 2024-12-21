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

    public AdminZoneEditPageViewModel(IZoneDataService zoneDataService, IDialogService dialogService)
    {
        _zoneDataService = zoneDataService;
        _dialogService = dialogService;

        UpdateCommand = new AsyncRelayCommand(UpdateZoneAsync);
    }

    public void NavigateBack()
    {
        App.GetService<INavigationService>().GoBack();
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

    public Task OnNavigatedTo(object parameter)
    {
        if (parameter is ZoneViewModel selectedZone)
        {
            Zone = selectedZone;
            Form.Name = Zone?.Name ?? string.Empty;
        }
        return Task.CompletedTask;
    }

    public bool CanUpdate()
    {
        return !Form.HasErrors && !IsProcessing;
    }

    public Task OnNavigatedFrom()
    {
        return Task.CompletedTask;
    }
}