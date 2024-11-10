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
public sealed partial class AdminZoneAddPage : Page
{
    public AdminZoneAddPageViewModel ViewModel { get; set; } = new AdminZoneAddPageViewModel();

    public AdminZoneAddPage()
    {
        ViewModel = App.GetService<AdminZoneAddPageViewModel>();
        DataContext = ViewModel;
        InitializeComponent();

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

    private void AdminZoneAddPage_CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Frame.GoBack();
    }

    private void Text_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            ViewModel.AddCommand.Execute(null);
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

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
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
        Frame.GoBack();
    }

    private async void OnShowErrorDialog(object sender, string message)
    {
        var dialog = new ContentDialog
        {
            XamlRoot = XamlRoot,
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
