using BistroQ.Presentation.ViewModels.Client;
using BistroQ.Presentation.ViewModels.Models;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views.Client;

public sealed partial class HomePage : Page
{
    public HomePageViewModel ViewModel { get; }

    public HomePage()
    {
        ViewModel = App.GetService<HomePageViewModel>();
        DataContext = ViewModel;
        InitializeComponent();
    }

    private void ProductList_AddProductToCart(object sender, ProductViewModel product)
    {
        ViewModel.AddProductToCartCommand.Execute(product);
    }
}