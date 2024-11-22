using BistroQ.Contracts.Services;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Services;

public class DialogService : IDialogService
{
    public async Task ShowDialogAsync(ContentDialog dialog)
    {
        await dialog.ShowAsync();
    }
}
