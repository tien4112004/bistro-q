using BistroQ.Presentation.Contracts.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Services;

/// <summary>
/// Implements IDialogService to handle the display of various dialog boxes in the application.
/// Ensures thread-safe dialog display and proper cleanup of resources.
/// </summary>
public class DialogService : IDialogService
{
    #region Private Fields
    /// <summary>
    /// Semaphore to ensure only one dialog is shown at a time.
    /// </summary>
    private static readonly SemaphoreSlim _semaphore = new(1);

    /// <summary>
    /// Cancellation token source for the currently displayed dialog.
    /// </summary>
    private CancellationTokenSource? _currentDialogCts;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the DialogService class.
    /// </summary>
    public DialogService()
    {
    }
    #endregion

    #region Public Methods
    /// <inheritdoc/>
    public async Task<ContentDialogResult> ShowConfirmDeleteDialog()
    {
        var dialog = new ContentDialog
        {
            XamlRoot = App.MainWindow.Content.XamlRoot,
            Title = "Confirm delete",
            Content = "Are you sure you want to delete this entry?",
            PrimaryButtonText = "Delete",
            SecondaryButtonText = "Cancel",
            SecondaryButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style
        };
        return await ShowDialogCoreAsync(dialog);
    }

    /// <inheritdoc/>
    public async Task ShowSuccessDialog(string message, string title = "Success")
    {
        var dialog = new ContentDialog
        {
            XamlRoot = App.MainWindow.Content.XamlRoot,
            Title = title,
            Content = message,
            CloseButtonText = "OK"
        };
        await ShowDialogCoreAsync(dialog);
    }

    /// <inheritdoc/>
    public async Task ShowErrorDialog(string message, string title = "Error")
    {
        var dialog = new ContentDialog
        {
            XamlRoot = App.MainWindow.Content.XamlRoot,
            Title = title,
            Content = message,
            CloseButtonText = "OK"
        };
        await ShowDialogCoreAsync(dialog);
    }

    /// <inheritdoc/>
    public async Task<ContentDialogResult> ShowDialogAsync(ContentDialog dialog)
    {
        dialog.XamlRoot = App.MainWindow.Content.XamlRoot;
        return await ShowDialogCoreAsync(dialog);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Core implementation for displaying dialogs with proper synchronization and cancellation handling.
    /// </summary>
    /// <param name="dialog">The content dialog to display.</param>
    /// <returns>A task representing the asynchronous operation with the dialog result.</returns>
    /// <remarks>
    /// This method ensures that:
    /// - Only one dialog is shown at a time using a semaphore
    /// - Previous dialogs are properly cancelled before showing a new one
    /// - Resources are properly cleaned up after the dialog is closed
    /// </remarks>
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
    #endregion
}