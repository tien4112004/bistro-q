using Microsoft.UI.Xaml.Data;

namespace BistroQ.Presentation.Converters;

/// <summary>
/// Converter that extracts the first validation error message from a dictionary of validation errors
/// for a specific property name provided as a parameter.
/// </summary>
public class FirstValidationErrorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        string propertyName = parameter as string;

        if (value is Dictionary<string, List<string>> errors)
        {
            if (errors.ContainsKey(propertyName))
            {
                return errors[propertyName].FirstOrDefault();
            }
        }

        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
