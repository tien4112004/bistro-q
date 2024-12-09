using BistroQ.Presentation.Contracts.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Services;

public class DialogService : IDialogService
{
    private readonly XamlRoot _xamlRoot;
    private static readonly SemaphoreSlim _semaphore = new(1);


    public DialogService()
    {
        _xamlRoot = App.MainWindow.Content.XamlRoot;
    }

    public async Task<ContentDialogResult> ShowConfirmDeleteDialog()
    {
        var dialog = new ContentDialog
        {
            XamlRoot = _xamlRoot,
            Title = "Confirm delete",
            Content = "Are you sure you want to delete this entry?",
            PrimaryButtonText = "Delete",
            SecondaryButtonText = "Cancel",
            SecondaryButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style
        };

        await _semaphore.WaitAsync();
        try
        {
            return await dialog.ShowAsync();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task ShowSuccessDialog(string message, string title = "Success")
    {
        var dialog = new ContentDialog
        {
            XamlRoot = _xamlRoot,
            Title = title,
            Content = message,
            CloseButtonText = "OK"
        };
        await _semaphore.WaitAsync();
        try
        {
            await dialog.ShowAsync();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task ShowErrorDialog(string message, string title = "Error")
    {
        var dialog = new ContentDialog
        {
            XamlRoot = _xamlRoot,
            Title = title,
            Content = message,
            CloseButtonText = "OK"
        };
        await _semaphore.WaitAsync();
        try
        {
            await dialog.ShowAsync();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task ShowDialogAsync(ContentDialog dialog)
    {
        dialog.XamlRoot = _xamlRoot;
        await _semaphore.WaitAsync();
        try
        {
            await dialog.ShowAsync();
        }
        finally
        {
            _semaphore.Release();
        }
    }
}