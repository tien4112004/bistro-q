using Microsoft.UI.Xaml.Data;

namespace BistroQ.Converters;

public class NullableIntToDoubleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int nullableInt)
        {
            return (double)nullableInt;
        }
        return 0.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is double doubleValue)
        {
            return (int?)doubleValue;
        }
        return null;
    }
}
