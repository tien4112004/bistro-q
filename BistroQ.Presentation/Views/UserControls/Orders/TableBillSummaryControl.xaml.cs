using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
    
    public event EventHandler CheckoutRequested;

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        CheckoutRequested?.Invoke(this, EventArgs.Empty);
    }
}