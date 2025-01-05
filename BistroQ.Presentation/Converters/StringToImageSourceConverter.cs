using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;

namespace BistroQ.Presentation.Converters;

/// <summary>
/// Converter that transforms string URLs or file paths into ImageSource objects.
/// Supports both web URLs and local file paths, handling various error cases gracefully.
/// </summary>
public class StringToImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string imageUrl && !string.IsNullOrEmpty(imageUrl))
        {
            try
            {
                if (imageUrl.StartsWith("file:") || System.IO.File.Exists(imageUrl))
                {
                    return new BitmapImage(new Uri(imageUrl));
                }
                else if (Uri.TryCreate(imageUrl, UriKind.Absolute, out Uri uri))
                {
                    return new BitmapImage(uri);
                }
            }
            catch
            {
                return null;
            }
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
