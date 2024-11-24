using Microsoft.UI.Xaml.Data;

namespace BistroQ.Converters;

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
