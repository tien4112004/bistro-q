using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Domain.Models.Entities;

public class OrderItem
{
    public int OrderDetailId { get; set; }

    public string? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Total => Quantity * PriceAtPurchase ?? 0;

    public decimal? PriceAtPurchase { get; set; }

    public Product Product { get; set; }
}
