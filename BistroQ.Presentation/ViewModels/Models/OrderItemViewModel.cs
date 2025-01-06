using BistroQ.Domain.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

/// <summary>
/// ViewModel representing an individual item within an order.
/// </summary>
/// <remarks>
/// Tracks the item's status, quantity, pricing, and relationships to products, orders, and tables.
/// </remarks>
public partial class OrderItemViewModel : ObservableObject
{
    /// <summary>
    /// Gets or sets the unique identifier for the order item
    /// </summary>
    [ObservableProperty]
    private string _orderItemId;

    /// <summary>
    /// Gets or sets the associated order ID
    /// </summary>
    [ObservableProperty]
    private string? _orderId;

    /// <summary>
    /// Gets or sets the associated product ID
    /// </summary>
    [ObservableProperty]
    private int? _productId;

    /// <summary>
    /// Gets or sets the quantity ordered
    /// </summary>
    [ObservableProperty]
    private int? _quantity;

    /// <summary>
    /// Gets or sets the current status of the order item
    /// </summary>
    [ObservableProperty]
    private OrderItemStatus? _status;

    /// <summary>
    /// Gets or sets the creation timestamp
    /// </summary>
    [ObservableProperty]
    private DateTime? _createdAt;

    /// <summary>
    /// Gets or sets the last update timestamp
    /// </summary>
    [ObservableProperty]
    private DateTime? _updatedAt;

    /// <summary>
    /// Gets or sets the price at time of purchase
    /// </summary>
    [ObservableProperty]
    private decimal? _priceAtPurchase;

    /// <summary>
    /// Gets or sets the associated table information
    /// </summary>
    [ObservableProperty]
    private TableViewModel? _table;

    /// <summary>
    /// Gets the total price for this item (quantity * price)
    /// </summary>
    public decimal? Total => Quantity * PriceAtPurchase;

    /// <summary>
    /// Gets or sets the parent order information
    /// </summary>
    [ObservableProperty]
    private OrderViewModel? _order;

    /// <summary>
    /// Gets or sets the associated product information
    /// </summary>
    [ObservableProperty]
    private ProductViewModel? _product;
}
