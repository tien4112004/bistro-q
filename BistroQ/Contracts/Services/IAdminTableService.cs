using BistroQ.Core.Dtos;
using BistroQ.Core.Dtos.Tables;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Contracts.Services;

public interface IAdminTableService
{
    Task<PaginationResponseDto<IEnumerable<TableDto>>> GetTablesAsync(TableCollectionQueryParams query);
    Task<bool> DeleteTableAsync(int zoneId);
    Task<ContentDialogResult> ShowConfirmDeleteDialog(XamlRoot xamlRoot);
    Task ShowSuccessDialog(string message, XamlRoot xamlRoot);
    Task ShowErrorDialog(string message, XamlRoot xamlRoot);
}
