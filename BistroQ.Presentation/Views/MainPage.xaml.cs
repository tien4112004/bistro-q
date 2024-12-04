using BistroQ.Presentation.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }

}
