using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.AdminZone;

public partial class AdminZoneAddPageViewModel : ObservableRecipient, INavigationAware
{
    [ObservableProperty]
    private bool _isProcessing = false;
    [ObservableProperty]
    private AddZoneForm _form = new();
    [ObservableProperty]
    private string _errorMessage = string.Empty;

    private readonly IZoneDataService _zoneDataService;
    private readonly IDialogService _dialogService;

    public ICommand AddCommand { get; }

    public event EventHandler NavigateBack;

    public AdminZoneAddPageViewModel(IZoneDataService zoneDataService, IDialogService dialogService)
    {
        _zoneDataService = zoneDataService;
        _dialogService = dialogService;

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
        Form.ClearErrors();
    }

    public void OnNavigatedFrom()
    {
        //
    }
}