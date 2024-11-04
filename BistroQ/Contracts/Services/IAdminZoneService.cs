using BistroQ.Core.Dtos.Zones;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Contracts.Services;

public interface IAdminZoneService
{
    Task<(IEnumerable<ZoneDto> Data, int TotalItems, int TotalPages,
          int CurrentPage)>
    GetZonesAsync(ZoneCollectionQueryParams query);
    Task<bool> DeleteZoneAsync(int zoneId);
    Task<ContentDialogResult> ShowConfirmDeleteDialog(XamlRoot xamlRoot);
    Task ShowSuccessDialog(string message, XamlRoot xamlRoot);
    Task ShowErrorDialog(string message, XamlRoot xamlRoot);
}