using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Core.Models.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message, Exception inner)
        : base(message, inner) { }

    public UnauthorizedException(string message) : base(message) { }
}
