﻿using Microsoft.UI.Xaml.Data;

namespace BistroQ.Converters;

public class DoubleToInt32Converter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is double doubleValue)
        {
            return (int)doubleValue;
        }
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is int intValue)
        {
            return (double)intValue;
        }
        return 0.0;
    }
}
