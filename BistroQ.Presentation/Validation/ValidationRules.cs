namespace BistroQ.Presentation.Validation;

/// <summary>
/// Provides a collection of validation rules for different data types.
/// These rules can be chained together using the Validate extension method.
/// </summary>
public static class ValidationRules
{
    /// <summary>
    /// Contains validation rules specific to integer values.
    /// </summary>
    public static class IntRules
    {
        /// <summary>
        /// Validates that an integer value is not null.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="fieldName">The name of the field being validated, used in error message</param>
        /// <returns>A tuple containing:
        /// - IsValid: true if the value is not null, false otherwise
        /// - Message: An error message if validation fails, empty string if validation succeeds
        /// </returns>
        public static (bool IsValid, string Message) NotEmpty(object? value, string fieldName)
        {
            return value is null
                ? (false, $"{fieldName} is required")
                : (true, string.Empty);
        }
    }

    /// <summary>
    /// Contains validation rules specific to string values.
    /// Provides common string validation scenarios like length checks,
    /// whitespace validation, and digit-only validation.
    /// </summary>
    public static class StringRules
    {
        /// <summary>
        /// Validates that a string value is not null, empty, or whitespace.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="fieldName">The name of the field being validated, used in error message</param>
        /// <returns>A tuple containing:
        /// - IsValid: true if the string has content, false otherwise
        /// - Message: An error message if validation fails, empty string if validation succeeds
        /// </returns>
        public static (bool IsValid, string Message) NotEmpty(object? value, string fieldName)
        {
            var strValue = value as string;
            return string.IsNullOrWhiteSpace(strValue)
                ? (false, $"{fieldName} is required")
                : (true, string.Empty);
        }

        /// <summary>
        /// Validates that a string meets the minimum length requirement.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="minLength">The minimum allowed length</param>
        /// <param name="fieldName">The name of the field being validated, used in error message</param>
        /// <returns>A tuple containing:
        /// - IsValid: true if the string is at least minLength characters long, false otherwise
        /// - Message: An error message if validation fails, empty string if validation succeeds
        /// </returns>
        public static (bool IsValid, string Message) MinLength(object? value, int minLength, string fieldName)
        {
            var strValue = value as string ?? string.Empty;
            return strValue.Length < minLength
                ? (false, $"{fieldName} must be at least {minLength} characters long")
                : (true, string.Empty);
        }

        /// <summary>
        /// Validates that a string does not exceed the maximum length.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="maxLength">The maximum allowed length</param>
        /// <param name="fieldName">The name of the field being validated, used in error message</param>
        /// <returns>A tuple containing:
        /// - IsValid: true if the string is not longer than maxLength characters, false otherwise
        /// - Message: An error message if validation fails, empty string if validation succeeds
        /// </returns>
        public static (bool IsValid, string Message) OnlyDigit(object? value, string fieldName)
        {
            var strValue = value as string ?? string.Empty;
            bool isDigitOnly = strValue.All(char.IsDigit);
            return isDigitOnly
                ? (true, string.Empty)
                : (false, $"{fieldName} must contain only digits");
        }

        /// <summary>
        /// Validates that a string does not exceed the maximum length.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="maxLength">The maximum allowed length</param>
        /// <param name="fieldName">The name of the field being validated, used in error message</param>
        /// <returns>A tuple containing:
        /// - IsValid: true if the string is not longer than maxLength characters, false otherwise
        /// - Message: An error message if validation fails, empty string if validation succeeds
        /// </returns>
        public static (bool IsValid, string Message) MaxLength(object? value, int maxLength, string fieldName)
        {
            var strValue = value as string ?? string.Empty;
            return strValue.Length > maxLength
                ? (false, $"{fieldName} must be at most {maxLength} characters long")
                : (true, string.Empty);
        }

        /// <summary>
        /// Validates that a string does not contain any whitespace characters.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="fieldName">The name of the field being validated, used in error message</param>
        /// <returns>A tuple containing:
        /// - IsValid: true if the string contains no whitespace, false otherwise
        /// - Message: An error message if validation fails, empty string if validation succeeds
        /// </returns>
        public static (bool IsValid, string Message) NoWhitespace(object? value, string fieldName)
        {
            var strValue = value as string ?? string.Empty;
            return strValue.Contains(' ')
                ? (false, $"{fieldName} must not contain whitespace")
                : (true, string.Empty);
        }
    }

    /// <summary>
    /// Contains validation rules specific to decimal values.
    /// Provides validations for required fields and minimum value checks.
    /// </summary>
    public static class DecimalRules
    {
        /// <summary>
        /// Validates that a decimal value is not null.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="fieldName">The name of the field being validated, used in error message</param>
        /// <returns>A tuple containing:
        /// - IsValid: true if the value is not null, false otherwise
        /// - Message: An error message if validation fails, empty string if validation succeeds
        /// </returns>
        public static (bool IsValid, string Message) NotEmpty(object? value, string fieldName)
        {
            return value is null
                ? (false, $"{fieldName} is required")
                : (true, string.Empty);
        }

        /// <summary>
        /// Validates that a decimal value is greater than or equal to the specified minimum.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="minValue">The minimum allowed value</param>
        /// <param name="fieldName">The name of the field being validated, used in error message</param>
        /// <returns>A tuple containing:
        /// - IsValid: true if the value is greater than or equal to minValue, false otherwise
        /// - Message: An error message if validation fails, empty string if validation succeeds
        /// </returns>

        public static (bool IsValid, string Message) Min(object? value, decimal minValue, string fieldName)
        {
            return value is decimal decimalValue && decimalValue < minValue
                ? (false, $"{fieldName} must be greater than or equal to {minValue}")
                : (true, string.Empty);
        }
    }
}
