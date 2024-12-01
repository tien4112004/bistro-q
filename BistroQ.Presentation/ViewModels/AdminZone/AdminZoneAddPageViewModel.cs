using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Models;
using BistroQ.Presentation.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.AdminZone;

public partial class AdminZoneAddPageViewModel : ObservableRecipient, INavigationAware
{
    private CreateZoneRequest _request;
    [ObservableProperty]
    private bool _isProcessing = false;
    [ObservableProperty]
    private AddZoneForm _form = new();
    [ObservableProperty]
    private string _errorMessage = string.Empty;

    private readonly IZoneDataService _zoneDataService;
    private readonly IAdminDialogService _adminDialogService;

    public ICommand AddCommand { get; }

    public event EventHandler NavigateBack;

    public AdminZoneAddPageViewModel(IZoneDataService zoneDataService)
    {
        _request = new CreateZoneRequest();
        _zoneDataService = zoneDataService;
        _adminDialogService = new AdminZoneDialogService();

        AddCommand = new AsyncRelayCommand(AddZoneAsync, CanAdd);
    }

    public bool CanAdd()
    {
        return !Form.HasErrors && !IsProcessing;
    }

    public async Task AddZoneAsync()
    {
        Form.ValidateAll();
        if (!CanAdd())
        {
            await _adminDialogService.ShowErrorDialog("Data is invalid. Please check again.");
            return;
        }

        try
        {
            IsProcessing = true;
            ErrorMessage = string.Empty;
            _request.Name = Form.Name;

            await _zoneDataService.CreateZoneAsync(_request);

            await _adminDialogService.ShowErrorDialog("Successfully added zone: " + _request.Name);
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
        Form.ClearErrors();
    }

    public void OnNavigatedFrom()
    {
        //
    }
}