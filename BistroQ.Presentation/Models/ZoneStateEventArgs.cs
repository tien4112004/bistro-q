using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Presentation.Models;

public class ZoneStateEventArgs
{
    public int? ZoneId { get; set; }
    public string Type { get; set; }
}
