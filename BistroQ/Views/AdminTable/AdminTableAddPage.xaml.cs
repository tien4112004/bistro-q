using BistroQ.ViewModels.AdminTable;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Views.AdminTable;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AdminTableAddPage : Page
{
    public AdminTableAddPageViewModel ViewModel { get; set; } = new AdminTableAddPageViewModel();

    public AdminTableAddPage()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<AdminTableAddPageViewModel>();
        this.DataContext = ViewModel;

        this.Loaded += AdminTableAddPage_Loaded;
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

    private async void AdminTableAddPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadZonesAsync();
    }

    private void Text_Keydown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            ViewModel.AddCommand.Execute(null);
        }

        if (sender is TextBox textBox)
        {
            ViewModel.FormChangedCommand.Execute((textBox.Name, textBox.Text));
        }
    }

    private void TextBox_LosingFocus(UIElement sender, LosingFocusEventArgs args)
    {
        if (sender is TextBox textbox)
        {
            ViewModel.FormChangedCommand.Execute((textbox.Name, textbox.Text));
        }
    }

    private void AdminTableAddPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.GoBack();
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
