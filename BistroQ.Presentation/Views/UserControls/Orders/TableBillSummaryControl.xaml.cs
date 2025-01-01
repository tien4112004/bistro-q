using BistroQ.Presentation.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Presentation.Views.UserControls.Orders;

public sealed partial class TableBillSummaryControl : UserControl
{
    public TableBillSummaryControl()
    {
        this.InitializeComponent();
    }

    public decimal? Total
    {
        get => (decimal?)GetValue(TotalProperty);
        set => SetValue(TotalProperty, value);
    }

    public string? ButtonText
    {
        get => (string?)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    public bool IsButtonEnabled
    {
        get => (bool)GetValue(IsButtonEnabledProperty);
        set => SetValue(IsButtonEnabledProperty, value);
    }

    public static readonly DependencyProperty TotalProperty =
        DependencyProperty.Register(
            nameof(Total),
            typeof(decimal?),
            typeof(TableBillSummaryControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ButtonTextProperty =
        DependencyProperty.Register(
            nameof(ButtonText),
            typeof(string),
            typeof(TableBillSummaryControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty IsButtonEnabledProperty =
        DependencyProperty.Register(
            nameof(IsButtonEnabled),
            typeof(bool),
            typeof(TableBillSummaryControl),
            new PropertyMetadata(true));

    public event EventHandler CheckoutRequested;

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        CheckoutRequested?.Invoke(this, EventArgs.Empty);
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