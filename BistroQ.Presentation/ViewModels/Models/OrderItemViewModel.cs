using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

public partial class OrderItemViewModel : ObservableObject
{
    [ObservableProperty]
    private string _orderItemId;

    [ObservableProperty]
    private string? _orderId;

    [ObservableProperty]
    private int? _productId;

    [ObservableProperty]
    private int? _quantity;

    [ObservableProperty]
    private decimal? _priceAtPurchase;

    [ObservableProperty]
    private string? _status;

    public decimal? Total => Quantity * PriceAtPurchase;

    [ObservableProperty]
    private ProductViewModel? _product;
}