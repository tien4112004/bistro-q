using BistroQ.Presentation.ViewModels.AdminTable;
using BistroQ.Presentation.ViewModels.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Presentation.Views.AdminTable;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AdminTableEditPage : Page
{
    public AdminTableEditPageViewModel ViewModel { get; set; }

    public AdminTableEditPage()
    {
        InitializeComponent();
        ViewModel = App.GetService<AdminTableEditPageViewModel>();
        this.DataContext = ViewModel;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var tableDto = e.Parameter as TableViewModel;
        if (tableDto != null)
        {
            ViewModel.Table = tableDto;
        }

        base.OnNavigatedTo(e);
    }

    private async void AdminTableEditPage_EditButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        try
        {
            var result = await ViewModel.UpdateTableAsync();

            await new ContentDialog()
            {
                XamlRoot = this.Content.XamlRoot,
                Title = "Update table successfully",
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
                Content = $"Update table failed with error: {ex.Message}",
                CloseButtonText = "OK"
            }.ShowAsync();
        }
    }

    private void AdminTableEditPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.GoBack();
    }

}