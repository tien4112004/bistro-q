using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Models.Entities;
using BistroQ.ViewModels;

using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

namespace BistroQ.Views;

public sealed partial class MainPage : Page
{
    private readonly IApiClient _apiClient;

    public MainViewModel ViewModel { get; }

    public MainPage()
    {
        _apiClient = App.GetService<IApiClient>();
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }


    private async void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        try
        {
            var res = await _apiClient.GetAsync<Product>($"api/Product/{Index.Text}", null);

            if (res.Success)
            {
                Text.Text = $"Product Name: {res.Data.Name}";

            }
            else
            {
                Text.Text = $"Error: {res.Message}";

            }
        }
        catch (Exception ex)
        {
            Text.Text = ex.Message;

            await Task.Delay(2000);

            App.MainWindow.Close();
        }
    }
}
