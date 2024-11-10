using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos.Tables;
using BistroQ.ViewModels.AdminTable;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Views.AdminTable;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AdminTableEditPage : Page
{
    public AdminTableEditPageViewModel ViewModel { get; set; } = new AdminTableEditPageViewModel();

    public AdminTableEditPage()
    {
        InitializeComponent();
        var tableDataService = App.GetService<ITableDataService>();
        var zoneDataService = App.GetService<IZoneDataService>();
        ViewModel = new AdminTableEditPageViewModel(tableDataService, zoneDataService);
        this.DataContext = ViewModel;

        this.Loaded += AdminTableEditPage_Loaded;
        ViewModel.ShowSuccessDialog += OnShowSuccessDialog;
        ViewModel.ShowErrorDialog += OnShowErrorDialog;
        ViewModel.NavigateBack += OnNavigateBack;

        this.Unloaded += (s, e) =>
        {
            ViewModel.ShowSuccessDialog -= OnShowSuccessDialog;
            ViewModel.ShowErrorDialog -= OnShowErrorDialog;
            ViewModel.NavigateBack -= OnNavigateBack;
        };
    }

    private async void AdminTableEditPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadZonesAsync();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var tableDto = e.Parameter as TableDto;
        if (tableDto != null)
        {
            ViewModel.Table = tableDto;
        }

        base.OnNavigatedTo(e);
    }

    private void AdminTableEditPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
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
