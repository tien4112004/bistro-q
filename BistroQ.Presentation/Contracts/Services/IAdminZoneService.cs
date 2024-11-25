using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Zones;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Contracts.Services;

public interface IAdminZoneService
{
    Task<PaginationResponse<IEnumerable<ZoneResponse>>> GetZonesAsync(ZoneCollectionQueryParams query);
    Task<bool> DeleteZoneAsync(int zoneId);
    Task<ContentDialogResult> ShowConfirmDeleteDialog(XamlRoot xamlRoot);
    Task ShowSuccessDialog(string message, XamlRoot xamlRoot);
    Task ShowErrorDialog(string message, XamlRoot xamlRoot);
}