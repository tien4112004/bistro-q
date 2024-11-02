using BistroQ.Core.Contracts.Services;
using BistroQ.ViewModels;

using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

namespace BistroQ.Views;

public sealed partial class MainPage : Page
{
    private readonly IAuthService _authService;
    private readonly ITokenStorageService _tokenStorageService;

    public MainViewModel ViewModel { get; }

    public MainPage()
    {
        _authService = App.GetService<IAuthService>();
        _tokenStorageService = App.GetService<ITokenStorageService>();
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }


    private async void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        try
        {   
            var token = await _tokenStorageService.GetAccessToken();

            if (string.IsNullOrEmpty(token))
            {
                Text.Text = "Not logged in";
            }
            else if (DateTime.Parse(token.Split(',')[1]) > DateTime.Now)
            {
                Text.Text = "Logged in";
            }
            else
            {
                Text.Text = "Token expired! Refresh It";

                await _authService.RefreshTokenAsync();
            }
        }
        catch (Exception)
        {
            Text.Text = "Refresh token expired! Logging out...";

            await Task.Delay(2000);

            await _authService.LogoutAsync();
        }
    }
}
