using BistroQ.Core.Dtos;
using BistroQ.Core.Dtos.Zones;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Contracts.Services;

public interface IAdminZoneService
{
    Task<PaginationResponseDto<IEnumerable<ZoneDto>>> GetZonesAsync(ZoneCollectionQueryParams query);
    Task<bool> DeleteZoneAsync(int zoneId);
    Task<ContentDialogResult> ShowConfirmDeleteDialog(XamlRoot xamlRoot);
    Task ShowSuccessDialog(string message, XamlRoot xamlRoot);
    Task ShowErrorDialog(string message, XamlRoot xamlRoot);
}