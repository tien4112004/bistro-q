using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views.UserControls.Client;

public sealed partial class SingleProductControl : UserControl
{
    public static readonly DependencyProperty ProductProperty =
        DependencyProperty.Register(
            nameof(Product),
            typeof(ProductViewModel),
            typeof(SingleProductControl),
            new PropertyMetadata(null));

    public ProductViewModel Product
    {
        get => (ProductViewModel)GetValue(ProductProperty);
        set => SetValue(ProductProperty, value);
    }

    public SingleProductControl()
    {
        this.InitializeComponent();
    }

    // public event EventHandler<ProductViewModel> AddProductToCart;
    public event EventHandler<ProductViewModel> ProductClicked;

    private void AddToCartButton_Click(object sender, RoutedEventArgs e)
    {

        if (sender is Button button && button.DataContext is ProductViewModel product)
        {
            // AddProductToCart?.Invoke(this, product);
            App.GetService<IMessenger>().Send(new AddProductToCartMessage(product));
        }
    }
}