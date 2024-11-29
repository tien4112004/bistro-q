using BistroQ.Domain.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

public partial class OrderItemViewModel : ObservableObject
{
    [ObservableProperty]
    private int _orderItemId;

    [ObservableProperty]
    private string? _orderId;

    [ObservableProperty]
    private int? _productId;

    [ObservableProperty]
    private int? _quantity;

    [ObservableProperty]
    private OrderItemStatus? _status;

    [ObservableProperty]
    private DateTime? _createdAt;

    [ObservableProperty]
    private DateTime? _updatedAt;

    [ObservableProperty]
    private decimal? _priceAtPurchase;

    public decimal? Total => Quantity * PriceAtPurchase;

    [ObservableProperty]
    private ProductViewModel? _product;
}