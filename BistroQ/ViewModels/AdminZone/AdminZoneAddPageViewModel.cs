using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos;
using BistroQ.Core.Dtos.Zones;
using BistroQ.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BistroQ.ViewModels.AdminZone;

public partial class AdminZoneAddPageViewModel : ObservableRecipient
{
    [ObservableProperty]
    private CreateZoneRequestDto request;
    [ObservableProperty]
    private bool _isProcessing = false;
    [ObservableProperty]
    private AddZoneForm _form = new();
    [ObservableProperty]
    private string _errorMessage = string.Empty;

    private readonly IZoneDataService _zoneDataService;

    public ICommand AddCommand { get; }
    public ICommand FormChangeCommand { get; }

    public event EventHandler<string> ShowSuccessDialog;
    public event EventHandler<string> ShowErrorDialog;
    public event EventHandler NavigateBack;

    public AdminZoneAddPageViewModel(IZoneDataService zoneDataService)
    {
        Request = new CreateZoneRequestDto();
        _zoneDataService = zoneDataService;
        AddCommand = new AsyncRelayCommand(async () => await Add(), CanAdd);

        FormChangeCommand = new RelayCommand<(string Field, string Value)>((param) =>
        {
            Form.ValidateProperty(param.Field, param.Value);
            ((AsyncRelayCommand)AddCommand).NotifyCanExecuteChanged();
        });
    }

    public AdminZoneAddPageViewModel()
    {
    }

    public async Task<ApiResponse<ZoneDto>> AddZone()
    {
        request.Name = Form.Name;
        if (string.IsNullOrEmpty(request.Name))
        {
            throw new InvalidDataException("Name cannot be null");
        }
        var result = await _zoneDataService.CreateZoneAsync(request);
        return result;
    }

    public bool CanAdd()
    {
        Form.ValidateAll();
        return !Form.HasErrors;
    }

    public async Task<ApiResponse<ZoneDto>> Add()
    {
        if (!CanAdd())
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

            var result = await AddZone();
            if (result.Success)
            {
                ShowSuccessDialog?.Invoke(this, $"Successfully added new zone: {Request.Name}");
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
}