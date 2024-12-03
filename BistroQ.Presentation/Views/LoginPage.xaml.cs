using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Presentation.Views;

public sealed partial class LoginPage : Page
{
    public LoginViewModel ViewModel { get; }

    private readonly Window _window;

    public LoginPage(Window window)
    {
        this.InitializeComponent();
        _window = window;
        ViewModel = App.GetService<LoginViewModel>();
        ViewModel.NavigationRequested += async (s, e) =>
        {
            await App.GetService<IActivationService>().ActivateAsync(EventArgs.Empty);
            window.Close();
        };

        ViewModel.CloseRequested += (s, e) => window.Close();
    }

    private async void On_Loaded(object sender, RoutedEventArgs e)
    {
        if (await ViewModel.IsAuthenticated())
        {
            ViewModel.RequestNavigation();
        }
    }

    private void On_UnLoaded(object sender, RoutedEventArgs e)
    {
        ViewModel.NavigationRequested -= async (s, e) =>
        {
            await App.GetService<IActivationService>().ActivateAsync(EventArgs.Empty);
            _window.Close();
        };

        ViewModel.CloseRequested -= (s, e) => _window.Close();
    }

    private void Text_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            ViewModel.LoginCommand.Execute(null);
        }
    }

    private void TextBox_LosingFocus(UIElement sender, LosingFocusEventArgs args)
    {
        switch (sender)
        {
            case TextBox textBox:
                ViewModel.FormChangeCommand.Execute((textBox.Name, textBox.Text));
                break;

            case PasswordBox passwordBox:
                ViewModel.FormChangeCommand.Execute((passwordBox.Name, passwordBox.Password));
                break;

            default:
                break;
        }
    }
}


