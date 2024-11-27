using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Contracts.Services;

public interface IAdminDialogService
{
    Task<ContentDialogResult> ShowConfirmDeleteDialog();
    Task ShowSuccessDialog(string message);
    Task ShowErrorDialog(string message);
}