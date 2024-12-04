using BistroQ.Presentation.ViewModels.CashierTable;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;


namespace BistroQ.Presentation.Views.UserControls.Orders;

public sealed partial class TableOrderDetailsControl : UserControl
{
    public TableOrderDetailViewModel ViewModel { get; set; }

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(
            nameof(ViewModel),
            typeof(TableOrderDetailViewModel),
            typeof(TableBillSummaryControl),
            new PropertyMetadata(null));

    public TableOrderDetailsControl()
    {
        this.InitializeComponent();
    }
    
    private void VerticalScrollViewer_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
    {
        const double SCROLL_SPEED = 1.25;
        var scrollViewer = (ScrollViewer)sender;
        scrollViewer.ChangeView(
            null,
            scrollViewer.VerticalOffset - e.Delta.Translation.Y * SCROLL_SPEED,
            null,
            true);
    }
}