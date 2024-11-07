using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos.Zones;
using BistroQ.ViewModels.AdminZone;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Views.AdminZone;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AdminZoneEditPage : Page
{
    public AdminZoneEditPageViewModel ViewModel { get; set; }

    public AdminZoneEditPage()
    {
        var zoneDataService = App.GetService<IZoneDataService>();
        ViewModel = new AdminZoneEditPageViewModel(zoneDataService);
        this.DataContext = ViewModel;
        this.InitializeComponent();

        ViewModel.ShowSuccessDialog += OnShowSuccessDialog;
        ViewModel.ShowErrorDialog += OnShowErrorDialog;
        ViewModel.NavigateBack += OnNavigateBack;

        Unloaded += (s, e) =>
        {
            ViewModel.ShowSuccessDialog -= OnShowSuccessDialog;
            ViewModel.ShowErrorDialog -= OnShowErrorDialog;
            ViewModel.NavigateBack -= OnNavigateBack;
        };
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is ZoneDto zoneDto)
        {
            ViewModel.OnNavigatedTo(zoneDto);
        }

        base.OnNavigatedTo(e);
    }

    private void Text_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            ViewModel.UpdateCommand.Execute(null);
        }
        if (sender is TextBox textBox)
        {
            ViewModel.FormChangeCommand.Execute((textBox.Name, textBox.Text));
        }
    }

    private void TextBox_LosingFocus(UIElement sender, LosingFocusEventArgs args)
    {
        if (sender is TextBox textBox)
        {
            ViewModel.FormChangeCommand.Execute((textBox.Name, textBox.Text));
        }
    }

    private void AdminZoneEditPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.GoBack();
    }

    private async void OnShowSuccessDialog(object sender, string message)
    {
        var dialog = new ContentDialog
        {
            XamlRoot = XamlRoot,
            Title = "Operation success",
            Content = message,
            CloseButtonText = "OK"
        };
        await dialog.ShowAsync();
    }

    private async void OnShowErrorDialog(object sender, string message)
    {
        var dialog = new ContentDialog
        {
            Title = "Error",
            Content = message,
            CloseButtonText = "OK"
        };
        await dialog.ShowAsync();
    }

    private void OnNavigateBack(object sender, EventArgs e)
    {
        Frame.GoBack();
    }
}
