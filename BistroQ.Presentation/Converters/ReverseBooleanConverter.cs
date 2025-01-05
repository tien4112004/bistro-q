using Microsoft.UI.Xaml.Data;

namespace BistroQ.Presentation.Converters;

/// <summary>
/// Converter that inverts boolean values.
/// Useful for scenarios where the opposite of a boolean property is needed.
/// </summary>
public class ReverseBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolean)
        {
            return !boolean;
        }

        return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
