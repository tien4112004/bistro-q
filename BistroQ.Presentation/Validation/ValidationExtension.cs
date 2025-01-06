namespace BistroQ.Presentation.Validation;

/// <summary>
/// Provides extension methods for validation operations.
/// </summary>
/// <remarks>
/// This class enables fluent validation by allowing multiple validation rules 
/// to be chained together using the Validate extension method.
/// 
/// Example usage:
/// <code>
/// value.Validate(
///     v => ValidationRules.StringRules.NotEmpty(v, "Username"),
///     v => ValidationRules.StringRules.MinLength(v, 3, "Username")
/// )
/// </code>
/// </remarks>
public static class ValidationExtension
{
    /// <summary>
    /// Applies multiple validation rules to a value and returns the validation results.
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <param name="validators">An array of validation functions to apply</param>
    /// <returns>An enumerable collection of validation results, where each result contains:
    /// - IsValid: Whether the validation passed
    /// - Message: The validation error message if validation failed, empty string otherwise</returns>
    /// <remarks>
    /// Each validator in the array is executed sequentially and their results are combined.
    /// This enables chaining multiple validation rules together in a fluent syntax.
    /// </remarks>
    public static IEnumerable<(bool IsValid, string Message)> Validate(this object? value, params Func<object?, (bool, string)>[] validators)
    {
        return validators.Select(validator => validator(value));
    }
}
