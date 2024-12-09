namespace BistroQ.Presentation.Validation;

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

        public static (bool IsValid, string Message) OnlyDigit(object? value, string fieldName)
        {
            var strValue = value as string ?? string.Empty;
            bool isDigitOnly = strValue.All(char.IsDigit);
            return isDigitOnly
                ? (true, string.Empty)
                : (false, $"{fieldName} must contain only digits");
        }
    }

    public static class DecimalRules
    {
        public static (bool IsValid, string Message) NotEmpty(object? value, string fieldName)
        {
            return value is null
                ? (false, $"{fieldName} is required")
                : (true, string.Empty);
        }

        public static (bool IsValid, string Message) Min(object? value, decimal minValue, string fieldName)
        {
            return value is decimal decimalValue && decimalValue < minValue
                ? (false, $"{fieldName} must be greater than or equal to {minValue}")
                : (true, string.Empty);
        }
    }
}
