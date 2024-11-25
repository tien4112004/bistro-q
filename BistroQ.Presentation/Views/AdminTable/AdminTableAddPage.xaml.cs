using BistroQ.Presentation.ViewModels.AdminTable;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Presentation.Views.AdminTable;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AdminTableAddPage : Page
{
    public AdminTableAddPageViewModel ViewModel { get; set; }

    public AdminTableAddPage()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<AdminTableAddPageViewModel>();
        this.DataContext = ViewModel;
        this.Loaded += AdminTableAddPage_Loaded;
    }

    private async void AdminTableAddPage_AddButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        try
        {
            var result = await ViewModel.AddTable();

            await new ContentDialog()
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Add new zone successfully",
                Content = "Successfully added zone: " + ViewModel.Request.SeatsCount,
                CloseButtonText = "OK"
            }.ShowAsync();
                
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

    private async void AdminTableAddPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadZonesAsync();
    }

    private void AdminTableAddPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.GoBack();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
    }
}