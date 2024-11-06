using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Validation;

public static class ValidationExtension
{
    public static IEnumerable<(bool IsValid, string Message)> Validate(this object? value, params Func<object?, (bool, string)>[] validators)
    {
        return validators.Select(validator => validator(value));
    }
}
