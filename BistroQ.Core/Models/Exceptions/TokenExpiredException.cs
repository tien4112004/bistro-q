using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Core.Models.Exceptions;

public class TokenExpiredException : Exception
{
    public TokenExpiredException(string message, Exception inner)
        : base(message, inner) { }

    public TokenExpiredException(string message) : base(message) { }
}
