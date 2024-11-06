using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Core.Models.Entities;

public class OrderDetail
{
    public int OrderDetailId { get; set; }

    public string? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? PriceAtPurchase { get; set; }
}
