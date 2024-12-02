using BistroQ.Core.Entities;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Views.UserControls.Client;

public sealed partial class SingleProductControl : UserControl
{
    public static readonly DependencyProperty ProductProperty =
        DependencyProperty.Register(
            nameof(Product),
            typeof(Product),
            typeof(SingleProductControl),
            new PropertyMetadata(null));

    public Product Product
    {
        get => (Product)GetValue(ProductProperty);
        set => SetValue(ProductProperty, value);
    }

    public SingleProductControl()
    {
        this.InitializeComponent();
    }

    public event EventHandler<Product> AddProductToCart;
    public event EventHandler<Product> ProductClicked;

    private void AddToCartButton_Click(object sender, RoutedEventArgs e)
    {

        if (sender is Button button && button.DataContext is Product product)
        {
            AddProductToCart?.Invoke(this, product);
        }
    }
}
