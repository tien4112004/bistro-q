using BistroQ.Presentation.Contracts.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Services;

public class AdminZoneDialogService : IAdminDialogService
{
    private readonly XamlRoot xamlRoot;

    public AdminZoneDialogService()
    {
        this.xamlRoot = App.MainWindow.Content.XamlRoot;
    }
    public async Task<ContentDialogResult> ShowConfirmDeleteDialog()
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

    public async Task ShowSuccessDialog(string message)
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

    public async Task ShowErrorDialog(string message)
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