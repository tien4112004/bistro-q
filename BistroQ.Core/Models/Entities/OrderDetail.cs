﻿using System.ComponentModel;

namespace BistroQ.Core.Entities;

public class OrderDetail : INotifyPropertyChanged
{
    public string OrderDetailId { get; set; } = null!;

    public string? OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal? PriceAtPurchase { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
}