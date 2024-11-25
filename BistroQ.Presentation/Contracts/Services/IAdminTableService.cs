using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Tables;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Contracts.Services;

public interface IAdminTableDialogService
{
    Task<ContentDialogResult> ShowConfirmDeleteDialog(XamlRoot xamlRoot);
    Task ShowSuccessDialog(string message, XamlRoot xamlRoot);
    Task ShowErrorDialog(string message, XamlRoot xamlRoot);
}