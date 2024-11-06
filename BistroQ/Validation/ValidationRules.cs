using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Validation;

public static class ValidationRules
{
    public static class StringRules
    {
        public static (bool IsValid, string Message) NotEmpty(object? value, string fieldName)
        {
            var strValue = value as string;
            return string.IsNullOrWhiteSpace(strValue)
                ? (false, $"{fieldName} is required")
                : (true, string.Empty);
        }

        public static (bool IsValid, string Message) MinLength(object? value, int minLength, string fieldName)
        {
            var strValue = value as string ?? string.Empty;
            return strValue.Length < minLength
                ? (false, $"{fieldName} must be at least {minLength} characters long")
                : (true, string.Empty);
        }
    }
}
