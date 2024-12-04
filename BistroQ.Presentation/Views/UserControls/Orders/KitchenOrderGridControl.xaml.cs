using BistroQ.Presentation.ViewModels.KitchenHistory;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;


namespace BistroQ.Presentation.Views.UserControls.Orders;

public sealed partial class KitchenOrderGridControl : UserControl
{
    public OrderItemGridViewModel ViewModel { get; set; } = null!;

    public static readonly DependencyProperty ViewModelProperty =
    DependencyProperty.Register(
        nameof(ViewModel),
        typeof(OrderItemGridViewModel),
        typeof(KitchenOrderGridControl),
        new PropertyMetadata(null));


    public KitchenOrderGridControl()
    {
        this.InitializeComponent();
    }
}
