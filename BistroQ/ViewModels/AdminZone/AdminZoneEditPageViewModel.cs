using BistroQ.Contracts.ViewModels;
using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos;
using BistroQ.Core.Dtos.Zones;
using BistroQ.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BistroQ.ViewModels.AdminZone;

public partial class AdminZoneEditPageViewModel : ObservableRecipient, INavigationAware
{
    public ZoneDto Zone { get; set; }
    [ObservableProperty]
    private UpdateZoneRequestDto request;
    [ObservableProperty]
    private bool _isProcessing = false;
    [ObservableProperty]
    private AddZoneForm _form = new();
    [ObservableProperty]
    private string _errorMessage = "";

    private readonly IZoneDataService _zoneDataService;

    public ICommand UpdateCommand { get; }
    public ICommand FormChangeCommand { get; }

    public event EventHandler<string> ShowErrorDialog;
    public event EventHandler<string> ShowSuccessDialog;
    public event EventHandler NavigateBack;

    public AdminZoneEditPageViewModel(IZoneDataService zoneDataService)
    {
        _zoneDataService = zoneDataService;
        Request = new UpdateZoneRequestDto();

        UpdateCommand = new AsyncRelayCommand(async () => await UpdateZone(), CanUpdate);
        FormChangeCommand = new RelayCommand<(string Field, string Value)>((param) =>
        {
            Form.ValidateProperty(param.Field, param.Value);
            ((AsyncRelayCommand)UpdateCommand).NotifyCanExecuteChanged();
        });
    }

    public bool CanUpdate()
    {
        Form.ValidateAll();
        return !Form.HasErrors;
    }


    public async Task<ApiResponse<ZoneDto>> UpdateZone()
    {
        if (!CanUpdate())
        {
            ShowErrorDialog?.Invoke(this, "Data is invalid. Please check again.");
            return new ApiResponse<ZoneDto>
            {
                Success = false,
                Error = "Data is invalid. Please check again."
            };
        }

        try
        {
            IsProcessing = true;
            ErrorMessage = string.Empty;

            request.Name = Form.Name;
            if (string.IsNullOrEmpty(request.Name))
            {
                throw new InvalidDataException("Name cannot be null");
            }
            var result = await _zoneDataService.UpdateZoneAsync(Zone.ZoneId.Value, request);
            if (result.Success)
            {
                ShowSuccessDialog?.Invoke(this, $"Successfully updated zone: {Request.Name}");
                NavigateBack?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ShowErrorDialog?.Invoke(this, result.Error);
            }
            return result;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            ShowErrorDialog?.Invoke(this, ex.Message);
        }
        finally
        {
            IsProcessing = false;
        }
        return new ApiResponse<ZoneDto>
        {
            Success = false,
            Error = "Something went wrong."
        };
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is ZoneDto selectedZone)
        {
            Zone = selectedZone;
            Form.Name = Zone?.Name ?? string.Empty;
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
