using BistroQ.ViewModels.AdminZone;
using Microsoft.UI.Xaml.Controls;

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
        this.DataContext = ViewModel;
        this.InitializeComponent();
    }

    private async void AdminZoneAddPage_AddButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        try
        {
            var result = await ViewModel.AddZone();

            if (result.Success)
            {
                await new ContentDialog()
                {
                    XamlRoot = this.Content.XamlRoot,
                    Title = "Add new zone successfully",
                    Content = "Successfully added zone: " + ViewModel.Request.Name,
                    CloseButtonText = "OK"
                }.ShowAsync();
            }
            Frame.GoBack();
        }
        catch (Exception ex)
        {
            await new ContentDialog()
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Operation failed",
                Content = $"Add new zone failed with error: {ex.Message}",
                CloseButtonText = "OK"
            }.ShowAsync();
        }
    }

    private void AdminZoneAddPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.GoBack();
    }
}
