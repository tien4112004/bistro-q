using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.ViewModels.AdminZone;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Presentation.Views.AdminZone;

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
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var zoneDto = e.Parameter as ZoneDto;
        if (zoneDto != null)
        {
            ViewModel.Zone = zoneDto;
        }

        base.OnNavigatedTo(e);
    }

    private async void AdminZoneEditPage_EditButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        try
        {
            var result = await ViewModel.UpdateZoneAsync();

            if (result.Success)
            {
                await new ContentDialog()
                {
                    XamlRoot = this.Content.XamlRoot,
                    Title = "Update zone successfully",
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
                Content = $"Update zone failed with error: {ex.Message}",
                CloseButtonText = "OK"
            }.ShowAsync();
        }
    }

    private void AdminZoneEditPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.GoBack();
    }
}
