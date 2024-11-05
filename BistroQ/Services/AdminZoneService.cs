using BistroQ.Contracts.Services;
using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos.Zones;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Services;

public class AdminZoneService : IAdminZoneService
{
    private readonly IZoneDataService _zoneDataService;

    public AdminZoneService(IZoneDataService zoneDataService)
    {
        _zoneDataService = zoneDataService;
    }

    public async Task<(IEnumerable<ZoneDto> Data, int TotalItems, int TotalPages, int CurrentPage)> GetZonesAsync(ZoneCollectionQueryParams query)
    {
        try
        {
            var result = await _zoneDataService.GetGridDataAsync(query);
            return (result.Data, result.TotalItems, result.TotalPages, result.CurrentPage);
        }
        catch (Exception ex)
        {
            // Log the error
            throw new ServiceException("Failed to retrieve zones.", ex);
        }
    }

    public async Task<bool> DeleteZoneAsync(int zoneId)
    {
        try
        {
            var result = await _zoneDataService.DeleteZoneAsync(zoneId);
            return result.Success;
        }
        catch (Exception ex)
        {
            // Log the error
            throw new ServiceException($"Failed to delete zone with ID {zoneId}.", ex);
        }
    }

    public async Task<ContentDialogResult> ShowConfirmDeleteDialog(XamlRoot xamlRoot)
    {
        var dialog = new ContentDialog
        {
            XamlRoot = xamlRoot,
            Title = "Confirm delete",
            Content = "Are you sure you want to delete this entry?",
            PrimaryButtonText = "Delete",
            SecondaryButtonText = "Cancel",
            SecondaryButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style
        };

        return await dialog.ShowAsync();
    }

    public async Task ShowSuccessDialog(string message, XamlRoot xamlRoot)
    {
        var dialog = new ContentDialog
        {
            XamlRoot = xamlRoot,
            Title = "Success",
            Content = message,
            CloseButtonText = "OK"
        };
        await dialog.ShowAsync();
    }

    public async Task ShowErrorDialog(string message, XamlRoot xamlRoot)
    {
        var dialog = new ContentDialog
        {
            XamlRoot = xamlRoot,
            Title = "Error",
            Content = message,
            CloseButtonText = "OK"
        };
        await dialog.ShowAsync();
    }
}

public class ServiceException : Exception
{
    public ServiceException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}