using BistroQ.Presentation.ViewModels.AdminAccount;
using BistroQ.Presentation.ViewModels.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace BistroQ.Presentation.Views.AdminAccount;

public sealed partial class AdminAccountEditPage : Page
{
    public AdminAccountEditPageViewModel ViewModel { get; set; }

    public AdminAccountEditPage()
    {
        InitializeComponent();
        ViewModel = App.GetService<AdminAccountEditPageViewModel>();
        this.DataContext = ViewModel;

        ViewModel.NavigateBack += OnNavigateBack;

        this.Unloaded += (s, e) =>
        {
            ViewModel.NavigateBack -= OnNavigateBack;
        };
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var accountDto = e.Parameter as AccountViewModel;
        if (accountDto != null)
        {
            ViewModel.Account = accountDto;
        }

        base.OnNavigatedTo(e);
    }

    private void AdminAccountEditPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.GoBack();
    }

    private void OnNavigateBack(object sender, EventArgs e)
    {
        Frame.GoBack();
    }

    private void AccountEditPage_UsernameTextBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.Username));
    }

    private void AccountEditPage_PasswordBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.Password));
    }

    private void AccountEditPage_RoleComboBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.Role));
    }

    private void AccountEditPage_ZoneComboBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.ZoneId));
    }

    private void AccountEditPage_TableComboBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.TableId));
    }
}