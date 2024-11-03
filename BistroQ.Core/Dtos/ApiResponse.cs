using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Core.Dtos;

public class ApiResponse<T> 
{
    public bool Success { get; set; }
    public T Data { get; set; }

    public string Message { get; set; }

    public string Error { get; set; }

    public int StatusCode { get; set; }
}
