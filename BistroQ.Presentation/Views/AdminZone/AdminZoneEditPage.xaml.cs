using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.ViewModels.AdminZone;
using BistroQ.Presentation.ViewModels.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace BistroQ.Presentation.Views.AdminZone;

public sealed partial class AdminZoneEditPage : Page
{
    public AdminZoneEditPageViewModel ViewModel { get; set; }

    public AdminZoneEditPage()
    {
        ViewModel = App.GetService<AdminZoneEditPageViewModel>();
        this.DataContext = ViewModel;
        this.InitializeComponent();

        ViewModel.NavigateBack += OnNavigateBack;

        Unloaded += (s, e) =>
        {
            ViewModel.NavigateBack -= OnNavigateBack;
        };
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var zoneDto = e.Parameter as ZoneViewModel;
        if (zoneDto != null)
        {
            ViewModel.Zone = zoneDto;
        }

        base.OnNavigatedTo(e);
    }

    private void OnNavigateBack(object sender, EventArgs e)
    {
        Frame.GoBack();
    }

    private void AdminZoneEditPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.GoBack();
    }
}