using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace BistroQ.Presentation.Converters;

/// <summary>
/// Converter that transforms boolean values to Visibility enumeration values.
/// Supports inversion of the conversion through a parameter.
/// </summary>
public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool booleanValue)
        {
            if (parameter is string stringParameter && stringParameter == "invert")
            {
                return booleanValue ? Visibility.Collapsed : Visibility.Visible;
            }

            return booleanValue ? Visibility.Visible : Visibility.Collapsed;
        }

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is Visibility visibility)
        {
            return visibility == Visibility.Visible;
        }

        return false;
    }
}
