using BistroQ.Presentation.ViewModels.CashierTable;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;



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
}
