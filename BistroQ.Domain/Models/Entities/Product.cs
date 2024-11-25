using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Domain.Models.Entities;

public class Product
{
    public int? ProductId { get; set; }

    public string Name { get; set; }

    public int? CategoryId { get; set; }

    public decimal? Price { get; set; }

    public string Unit { get; set; }

    public decimal? DiscountPrice { get; set; }

    public string ImageUrl { get; set; }
}
