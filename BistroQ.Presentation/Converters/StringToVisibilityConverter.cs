using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace BistroQ.Presentation.Converters;

/// <summary>
/// Converter that transforms strings to Visibility enumeration values
/// based on whether the string is null or empty.
/// </summary>
public class StringToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return string.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
