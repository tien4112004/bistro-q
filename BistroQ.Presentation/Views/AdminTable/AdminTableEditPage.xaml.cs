using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.ViewModels.AdminTable;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Presentation.Views.AdminTable;

public sealed partial class AdminTableEditPage : Page
{
    public AdminTableEditPageViewModel ViewModel { get; set; }

    public AdminTableEditPage()
    {
        InitializeComponent();
        ViewModel = App.GetService<AdminTableEditPageViewModel>();
        this.DataContext = ViewModel;

        this.Loaded += AdminTableEditPage_Loaded;
    }

    private async void AdminTableEditPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadZonesAsync();
        TableEditPage_ZoneComboBox.SelectedValue = ViewModel.Request.ZoneId;
    }

    private void AdminTableEditPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.NavigateBack();
    }

    private void OnNavigateBack(object sender, EventArgs e)
    {
        ViewModel.NavigateBack();
    }

    private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Hand);
    }

    private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Arrow);
    }
}