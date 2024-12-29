using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace BistroQ.Presentation.Converters;

public class ProgressBarValueToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is double progressValue && progressValue >= 100.0)
        {
            return (SolidColorBrush)Application.Current.Resources["SystemFillColorCriticalBrush"];
        }
        return (SolidColorBrush)Application.Current.Resources["AccentFillColorDefaultBrush"];
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
