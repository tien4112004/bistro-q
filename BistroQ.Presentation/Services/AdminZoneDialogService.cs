using BistroQ.Presentation.Contracts.Services;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Zones;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Services;

public class AdminZoneDialogService : IAdminZoneService
{
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