using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Core.Dtos;

public class Pagination
{
    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalItems { get; set; } = 0;
}
