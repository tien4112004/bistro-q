using BistroQ.Presentation.Contracts.Services;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Tables;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Services;

public class AdminTableService : IAdminTableService
{
    private readonly ITableDataService _zoneDataService;

    public AdminTableService(ITableDataService zoneDataService)
    {
        _zoneDataService = zoneDataService;
    }

    public async Task<PaginationResponse<IEnumerable<TableResponse>>> GetTablesAsync(TableCollectionQueryParams query)
    {
        try
        {
            var result = await _zoneDataService.GetGridDataAsync(query);
            return result;
        }
        catch (Exception ex)
        {
            // Log the error
            throw new ServiceException("Failed to retrieve zones.", ex);
        }
    }

    public async Task<bool> DeleteTableAsync(int zoneId)
    {
        try
        {
            var result = await _zoneDataService.DeleteTableAsync(zoneId);
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
