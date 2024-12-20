using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.ViewModels.AdminCategory;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Presentation.Views.AdminCategory;

public sealed partial class AdminCategoryAddPage : Page
{
    public AdminCategoryAddPageViewModel ViewModel { get; set; }

    public AdminCategoryAddPage()
    {
        ViewModel = App.GetService<AdminCategoryAddPageViewModel>();
        this.DataContext = ViewModel;
        this.InitializeComponent();
    }

    private void AdminCategoryAddPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.NavigateBack();
    }

    private void OnNavigateBack(object sender, EventArgs e)
    {
        ViewModel.NavigateBack();
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