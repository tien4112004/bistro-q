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
            await _authService.GetTokenAsync();
        }
        catch (Exception ex)
        {
            Text.Text = ex.Message;

            await Task.Delay(2000);

            App.MainWindow.Close();
        }
    }
}
