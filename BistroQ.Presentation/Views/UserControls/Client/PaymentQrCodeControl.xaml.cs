using BistroQ.Presentation.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using QRCoder;
using System.Windows.Input;
using Windows.Storage.Streams;

namespace BistroQ.Presentation.Views.UserControls.Client;

public sealed partial class PaymentQrCodeControl : UserControl
{
    public static readonly DependencyProperty PaymentDataProperty =
        DependencyProperty.Register(nameof(PaymentData), typeof(string), typeof(PaymentQrCodeControl),
            new PropertyMetadata(null, OnPaymentDataChanged));

    public static readonly DependencyProperty CancelCommandProperty =
        DependencyProperty.Register(nameof(CancelCommand), typeof(ICommand), typeof(PaymentQrCodeControl),
            new PropertyMetadata(null));

    public string PaymentData
    {
        get => (string)GetValue(PaymentDataProperty);
        set => SetValue(PaymentDataProperty, value);
    }

    public ICommand CancelCommand
    {
        get => (ICommand)GetValue(CancelCommandProperty);
        set => SetValue(CancelCommandProperty, value);
    }

    public PaymentQrCodeControl()
    {
        this.InitializeComponent();
    }

    private static void OnPaymentDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PaymentQrCodeControl control)
        {
            control.GenerateQRCode((string)e.NewValue);
        }
    }

    private async void GenerateQRCode(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            QRCodeImage.Source = null;
            return;
        }

        try
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new BitmapByteQRCode(qrCodeData);
            var qrCodeBytes = qrCode.GetGraphic(20);

            using var stream = new InMemoryRandomAccessStream();
            using (var writer = new DataWriter(stream.GetOutputStreamAt(0)))
            {
                writer.WriteBytes(qrCodeBytes);
                await writer.StoreAsync();
            }

            var bitmap = new BitmapImage();
            await bitmap.SetSourceAsync(stream);
            QRCodeImage.Source = bitmap;
        }
        catch (Exception ex)
        {
            QRCodeImage.Source = null;
        }
    }

    private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Hand);
    }

    private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Arrow);
    }
}