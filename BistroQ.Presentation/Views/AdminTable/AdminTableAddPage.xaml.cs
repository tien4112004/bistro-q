using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.ViewModels.AdminTable;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Presentation.Views.AdminTable;

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

    private void AdminTableAddPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        _ = ViewModel.LoadZonesAsync();
    }

    private void AdminTableAddPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.NavigateBack();
    }

    private void OnNavigateBack(object sender, EventArgs e)
    {
        ViewModel.NavigateBack();
    }

    private void TableAddPage_SeatsCountNumberBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.SeatsCount));
    }

    private void TableAddPage_ZoneComboBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.ZoneId));
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