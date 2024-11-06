using BistroQ.Core.Dtos.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Core.Models.Entities;

public class Order
{
    public string OrderId { get; set; } = null!;

    public decimal? TotalAmount { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? TableId { get; set; }

    public TableDto Table { get; set; } = null!;

    public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
