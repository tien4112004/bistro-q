using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Contracts.Services;

public interface IDialogService
{
    Task ShowDialogAsync(ContentDialog dialog);
}