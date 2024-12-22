using BistroQ.Presentation.Contracts.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Services;

public class DialogService : IDialogService
{
    private readonly XamlRoot _xamlRoot;
    private static readonly SemaphoreSlim _semaphore = new(1);
    private CancellationTokenSource? _currentDialogCts;

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

        return await ShowDialogCoreAsync(dialog);
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

        await ShowDialogCoreAsync(dialog);
    }

    public async Task ShowErrorDialog(string message, string title = "Error")
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK"
        };

        await ShowDialogCoreAsync(dialog);
    }

    public async Task ShowDialogAsync(ContentDialog dialog)
    {
        dialog.XamlRoot = _xamlRoot;

        await ShowDialogCoreAsync(dialog);
    }

    private async Task<ContentDialogResult> ShowDialogCoreAsync(ContentDialog dialog)
    {
        if (_currentDialogCts != null)
        {
            _currentDialogCts.Cancel();
            _currentDialogCts.Dispose();
        }

        _currentDialogCts = new CancellationTokenSource();

        await _semaphore.WaitAsync();
        try
        {
            _currentDialogCts.Token.ThrowIfCancellationRequested();
            return await dialog.ShowAsync();
        }
        catch (OperationCanceledException)
        {
            return ContentDialogResult.None;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}