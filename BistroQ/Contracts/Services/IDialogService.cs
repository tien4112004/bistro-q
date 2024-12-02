using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Contracts.Services;

public interface IDialogService
{
    Task ShowDialogAsync(ContentDialog dialog);
}
