using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.ViewModels.AdminZone;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Presentation.Views.AdminZone;

public sealed partial class AdminZoneAddPage : Page
{
    public AdminZoneAddPageViewModel ViewModel { get; set; }

    public AdminZoneAddPage()
    {
        ViewModel = App.GetService<AdminZoneAddPageViewModel>();
        this.DataContext = ViewModel;
        this.InitializeComponent();

        ViewModel.NavigateBack += OnNavigateBack;

        Unloaded += (s, e) =>
        {
            ViewModel.NavigateBack -= OnNavigateBack;
        };
    }

    private void AdminZoneAddPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.GoBack();
    }

    private void OnNavigateBack(object sender, EventArgs e)
    {
        Frame.GoBack();
    }

    private void Name_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.Name));
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