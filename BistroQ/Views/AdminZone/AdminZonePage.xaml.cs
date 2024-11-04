using BistroQ.Core.Dtos.Zones;
using BistroQ.ViewModels;
using BistroQ.Views.AdminZone;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace BistroQ.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class AdminZonePage : Page
{
    public AdminZoneViewModel ViewModel
    {
        get;
    }

    public AdminZonePage()
    {
        ViewModel = App.GetService<AdminZoneViewModel>();
        this.DataContext = ViewModel;
        InitializeComponent();
    }

    public void AddButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var addButton = sender as Button;
        if (addButton != null)
        {
            addButton.IsEnabled = false;
        }

        Frame.Navigate(typeof(AdminZoneAddPage));

        if (addButton != null)
        {
            addButton.IsEnabled = true;
        }
    }

    public void EditButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedZone = AdminZoneDataGrid.SelectedItem as ZoneDto;
        if (selectedZone != null)
        {
            Frame.Navigate(typeof(AdminZoneEditPage), selectedZone);
        }
    }

    public async void DeleteButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedZone = AdminZoneDataGrid.SelectedItem as ZoneDto;
        if (selectedZone != null)
        {
            var confirmDelete = await new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "Confirm delete",
                Content = "Are you sure to delete this entry?",
                PrimaryButtonText = "Delete",
                SecondaryButtonText = "Cancel",
                SecondaryButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style
            }.ShowAsync();

            if (confirmDelete == ContentDialogResult.Primary)
            {
                bool success = await ViewModel.DeleteZoneAsync(selectedZone.ZoneId.Value);

                var items = AdminZoneDataGrid.ItemsSource as ObservableCollection<ZoneDto>;

                if (success)
                {
                    var successDialog = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "Success",
                        Content = "Zone deleted successfully.",
                        CloseButtonText = "OK"
                    };
                    await successDialog.ShowAsync();
                    items.Remove(selectedZone);
                }
                else
                {
                    var errorDialog = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "Error",
                        Content = $"An error occured. Please try again.",
                        CloseButtonText = "OK"
                    };
                    await errorDialog.ShowAsync();
                }
            }
        }
    }
}
