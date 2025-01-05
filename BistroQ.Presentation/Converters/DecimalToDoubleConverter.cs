using Microsoft.UI.Xaml.Data;

namespace BistroQ.Presentation.Converters;

/// <summary>
/// Converter that handles conversion between decimal and double data types while maintaining precision
/// and handling edge cases like null values and out-of-range numbers.
/// </summary>
public partial class DecimalToDoubleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is decimal d)
        {
            return (double)d;
        }

        return 0.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return 0m;
        }

        if (value is double d && d >= (double)decimal.MinValue && d <= (double)decimal.MaxValue)
        {
            return (decimal)d;
        }

        var strValue = value.ToString();
        if (double.TryParse(strValue, out double result))
        {
            if (result >= (double)decimal.MinValue && result <= (double)decimal.MaxValue)
            {
                return (decimal)result;
            }
        }
        return 0m;
    }
}
