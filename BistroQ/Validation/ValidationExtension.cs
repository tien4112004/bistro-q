namespace BistroQ.Validation;

public static class ValidationExtension
{
    public static IEnumerable<(bool IsValid, string Message)> Validate(this object? value, params Func<object?, (bool, string)>[] validators)
    {
        return validators.Select(validator => validator(value));
    }
}
