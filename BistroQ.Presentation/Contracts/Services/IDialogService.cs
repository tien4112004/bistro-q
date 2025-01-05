using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Contracts.Services;

/// <summary>
/// Provides an interface for displaying various types of dialog boxes in the application.
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Displays a confirmation dialog for delete operations.
    /// </summary>
    /// <returns>A task representing the asynchronous operation with the dialog result.</returns>
    Task<ContentDialogResult> ShowConfirmDeleteDialog();

    /// <summary>
    /// Displays a success dialog with a custom message and title.
    /// </summary>
    /// <param name="message">The message to display in the dialog.</param>
    /// <param name="title">The title of the dialog.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ShowSuccessDialog(string message, string title);

    /// <summary>
    /// Displays an error dialog with a custom message and title.
    /// </summary>
    /// <param name="message">The message to display in the dialog.</param>
    /// <param name="title">The title of the dialog.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ShowErrorDialog(string message, string title);

    /// <summary>
    /// Displays a custom content dialog.
    /// </summary>
    /// <param name="dialog">The content dialog to display.</param>
    /// <returns>A task representing the asynchronous operation with the dialog result.</returns>
    Task<ContentDialogResult> ShowDialogAsync(ContentDialog dialog);
}