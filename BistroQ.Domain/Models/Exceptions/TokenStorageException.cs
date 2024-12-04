using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Domain.Models.Exceptions;

public class TokenStorageException : Exception
{
    public TokenStorageException(string message, Exception inner)
        : base(message, inner) { }

    public TokenStorageException(string message) : base(message) { }
}
