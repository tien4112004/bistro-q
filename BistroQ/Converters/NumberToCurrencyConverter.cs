﻿using Microsoft.UI.Xaml.Data;
using System.Globalization;

namespace BistroQ.Converters;

public class NumberToCurrencyConverter : IValueConverter
{
    private readonly CultureInfo _vietnameseCulture;

    public NumberToCurrencyConverter()
    {
        _vietnameseCulture = new CultureInfo("vi-VN");
    }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is decimal decimalValue)
        {
            return decimalValue.ToString("C", _vietnameseCulture);
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
