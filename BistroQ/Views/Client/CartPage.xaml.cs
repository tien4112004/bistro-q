using BistroQ.Core.Entities;
using BistroQ.ViewModels.Client;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace BistroQ.Views.Client;

public sealed partial class CartPage : Page
{
    private OrderCartViewModel ViewModel => (OrderCartViewModel)DataContext;

    public bool CartIsEmpty => ViewModel.CartItems == null || ViewModel.CartItems.Count == 0;

    public RelayCommand<Product> AddToCartCommand { get; set; }

    public CartPage()
    {
        this.InitializeComponent();
        DataContext = App.GetService<OrderCartViewModel>();
        AddToCartCommand = new RelayCommand<Product>(AddToCart);

        var productListViewModel = App.GetService<ProductListViewModel>();
        //productListViewModel.ProductAddedToCart += OnProductAddedToCart;

        ViewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModel.CartItems))
        {
            OnPropertyChanged(nameof(CartIsEmpty));
        }
    }

    private void AddToCart(Product product)
    {
        if (product == null) return;

        var existingOrderItem = ViewModel.CartItems.FirstOrDefault(od => od.ProductId == product.ProductId);
        if (existingOrderItem != null)
        {
            existingOrderItem.Quantity++;
        }
        else
        {
            ViewModel.CartItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                Product = product,
                Quantity = 1,
                PriceAtPurchase = product.Price
            });
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
