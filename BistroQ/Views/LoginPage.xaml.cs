using BistroQ.Contracts.Services;
using BistroQ.Services;
using BistroQ.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
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


