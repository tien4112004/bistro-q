using BistroQ.Core.Entities;
using BistroQ.ViewModels.Client;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Views.Client;

public sealed partial class HomePage : Page
{
    public HomePageViewModel ViewModel { get; }

    public HomePage()
    {
        ViewModel = App.GetService<HomePageViewModel>();
        DataContext = ViewModel;
        InitializeComponent();
    }

    private void ProductList_AddProductToCart(object sender, Product product)
    {
        ViewModel.AddProductToCartCommand.Execute(product);
    }
}
