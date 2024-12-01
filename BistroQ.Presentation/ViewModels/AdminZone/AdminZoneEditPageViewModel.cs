using System.Windows.Input;
using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Models;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BistroQ.Presentation.ViewModels.AdminZone;

public partial class AdminZoneEditPageViewModel : ObservableRecipient, INavigationAware
{
    public ZoneViewModel Zone { get; set; }
    [ObservableProperty]
    private UpdateZoneRequest _request;
    [ObservableProperty]
    private bool _isProcessing = false;
    [ObservableProperty]
    private AddZoneForm _form = new();
    [ObservableProperty]
    private string _errorMessage = "";
    
    public AdminZoneEditPageViewModel ViewModel;

    private readonly IZoneDataService _zoneDataService;
    private readonly IAdminDialogService _adminDialogService;

    public ICommand UpdateCommand { get; }

    public event EventHandler NavigateBack;
    public AdminZoneEditPageViewModel(IZoneDataService zoneDataService)
    {
        _zoneDataService = zoneDataService;
        Request = new UpdateZoneRequest();
        _zoneDataService = zoneDataService;
        
        UpdateCommand = new AsyncRelayCommand(async () => await UpdateZoneAsync(), CanUpdate);
    }

    public async Task UpdateZoneAsync()
    {
        Form.ValidateAll();
        if (!CanUpdate())
        {
            await _adminDialogService.ShowErrorDialog("Data is invalid. Please check again.");
            return;
        }

        try
        {
            IsProcessing = true;
            ErrorMessage = string.Empty;

            _request.Name = Form.Name;
            await _zoneDataService.UpdateZoneAsync(Zone.ZoneId.Value, _request);
            
            await _adminDialogService.ShowSuccessDialog($"Successfully updated zone: {Request.Name}");
            NavigateBack?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await _adminDialogService.ShowErrorDialog(ex.Message);
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
            Request.Name = Zone?.Name ?? string.Empty;
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