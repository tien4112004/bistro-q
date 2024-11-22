using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Converters;

public class NumberToCurrencyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int intValue)
        {
            return intValue.ToString("$");
        }
        else if (value is decimal decicmalValue)
        {
            return decicmalValue.ToString() + ("$");
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string stringValue)
        {
            if (int.TryParse(stringValue, out int result))
            {
                return result;
            }
        }
        return value;
    }
}
