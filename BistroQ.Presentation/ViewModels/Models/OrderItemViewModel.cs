using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

public partial class OrderItemViewModel : ObservableObject
{
    [ObservableProperty]
    private int _orderDetailId;

    [ObservableProperty]
    private string? _orderId;

    [ObservableProperty]
    private int? _productId;

    [ObservableProperty]
    private int? _quantity;

    [ObservableProperty]
    private decimal? _priceAtPurchase;
    
    [ObservableProperty]
    private ProductViewModel? _product;
}