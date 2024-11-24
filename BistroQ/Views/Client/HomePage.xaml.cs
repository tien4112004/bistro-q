using BistroQ.ViewModels.Client;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Views.Client;

public sealed partial class HomePage : Page
{
    public HomePageViewModel ViewModel { get; }

    public HomePage()
    {
        ViewModel = App.GetService<HomePageViewModel>();
        InitializeComponent();
    }
}
