using Microsoft.UI.Xaml.Data;

namespace BistroQ.Presentation.Converters;

public class DateTimeToTimeOnlyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        // Format: https://www.c-sharpcorner.com/blogs/date-and-time-format-in-c-sharp-programming1
        if (value is DateTime dateTime && parameter is string format)
        {
            return dateTime.ToString(format);
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }

}
