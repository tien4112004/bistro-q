using System.ComponentModel;

namespace BistroQ.Core.Entities;

public class OrderItem : INotifyPropertyChanged
{
    public string OrderItemId { get; set; } = null!;

    public string? OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal? Total => Quantity * PriceAtPurchase ?? 0;

    public decimal? PriceAtPurchase { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
}