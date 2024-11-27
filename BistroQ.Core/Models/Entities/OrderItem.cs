using System.ComponentModel;

namespace BistroQ.Core.Entities;

public class OrderItem : INotifyPropertyChanged
{
    public string OrderItemId { get; set; } = null;

    public string? OrderId { get; set; }

    public int ProductId { get; set; }

    private int _quantity;
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (_quantity != value)
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
    }
    public decimal? Total => Quantity * PriceAtPurchase ?? 0;

    public decimal? PriceAtPurchase { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}