using BistroQ.Domain.Dtos.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Domain.Models.Entities;

public class Order
{
    public string OrderId { get; set; } = null!;
    
    public decimal? TotalAmount { get; set; }
    
    public DateTime? StartTime { get; set; }
    
    public DateTime? EndTime { get; set; }

    public string Status { get; set; }

    public int PeopleCount { get; set; }
    
    public int? TableId { get; set; }
    
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}