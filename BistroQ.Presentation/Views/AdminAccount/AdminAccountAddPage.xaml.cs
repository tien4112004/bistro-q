using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.ViewModels.AdminAccount;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Presentation.Views.AdminAccount;

public sealed partial class AdminAccountAddPage : Page
{
    public AdminAccountAddPageViewModel ViewModel { get; set; }

    public AdminAccountAddPage()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<AdminAccountAddPageViewModel>();
        this.DataContext = ViewModel;

        this.Unloaded += (s, e) => ViewModel.Dispose();
    }

    private void AdminAccountAddPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.NavigateBack();
    }

    private void OnNavigateBack(object sender, EventArgs e)
    {
        ViewModel.NavigateBack();
    }

    private void AccountAddPage_UsernameTextBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.Username));
    }

    private void AccountAddPage_PasswordBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.Password));
    }

    private void AccountAddPage_RoleComboBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.Role));
    }

    private void AccountAddPage_ZoneComboBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.ZoneId));
    }

    private void AccountAddPage_TableComboBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.TableId));
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