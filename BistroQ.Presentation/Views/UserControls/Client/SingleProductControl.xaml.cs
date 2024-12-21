using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Presentation.Views.UserControls.Client;

public sealed partial class SingleProductControl : UserControl
{
    public bool HasDiscountPrice => Product?.DiscountPrice != 0;

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
        if (!HasDiscountPrice) ProductDescriptionTextBlock.Text = "";
        this.Tapped += SingleProductControl_Tapped;
    }

    // public event EventHandler<ProductViewModel> AddProductToCart;
    public event EventHandler<ProductViewModel> ProductClicked;

    private void AddToCartButton_Click(object sender, RoutedEventArgs e)
    {

        if (sender is Button button && button.DataContext is ProductViewModel product)
        {
            App.GetService<IMessenger>().Send(new AddProductToCartMessage(product));
        }
    }

    private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Hand);
    }

    private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Arrow);
    }

    private void SingleProductControl_Tapped(object sender, TappedRoutedEventArgs e)
    {
        ProductClicked?.Invoke(this, Product);
    }
}