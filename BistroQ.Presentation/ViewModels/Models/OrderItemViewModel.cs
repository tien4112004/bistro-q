using BistroQ.Domain.Enums;
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
    private OrderItemStatus? _status;

    [ObservableProperty]
    private DateTime? _createdAt;

    [ObservableProperty]
    private DateTime? _updatedAt;

    [ObservableProperty]
    private decimal? _priceAtPurchase;

    [ObservableProperty]
    private TableViewModel? _table;

    public decimal? Total => Quantity * PriceAtPurchase;

    [ObservableProperty]
    private OrderViewModel? _order;

    [ObservableProperty]
    private ProductViewModel? _product;
}