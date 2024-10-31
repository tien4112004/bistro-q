using BistroQ.Contracts.Services;
using BistroQ.Core.Contracts.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class LoginWindow : Window
{
    public LoginWindow()
    {
        this.InitializeComponent();
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            LoginButton.IsEnabled = false;
            ErrorMessageText.Visibility = Visibility.Collapsed;
            TextLogin.Visibility = Visibility.Collapsed;
            LoginProgressRing.IsActive = true;


            var result = await App.GetService<IAuthService>().LoginAsync(UsernameTextBox.Text, PasswordBox.Password);

            if (!result.Success)
            {
                ErrorMessageText.Text = result.Message;
                ErrorMessageText.Visibility = Visibility.Visible;
                return;
            }

            await App.GetService<IActivationService>().ActivateAsync(e);
            this.Close();
        }
        catch (Exception)
        {
            ErrorMessageText.Text = "An error occurred. Please try again.";
            ErrorMessageText.Visibility = Visibility.Visible;
        }
        finally
        {
            LoginButton.IsEnabled = true;
            TextLogin.Visibility = Visibility.Visible;
            LoginProgressRing.IsActive = false;
        }
    }
}
