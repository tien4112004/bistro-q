using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Contracts.Services;

public interface IDialogService
{
    Task<ContentDialogResult> ShowConfirmDeleteDialog();
    Task ShowSuccessDialog(string message, string title);
    Task ShowErrorDialog(string message, string title);
    Task ShowDialogAsync(ContentDialog dialog);
}