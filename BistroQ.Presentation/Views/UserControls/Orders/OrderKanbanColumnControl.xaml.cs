using BistroQ.Presentation.ViewModels.KitchenOrder;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views.UserControls.Orders;

public sealed partial class OrderKanbanColumnControl : UserControl
{
    public OrderKanbanColumnViewModel ViewModel { get; set; }

    public string? Title { get; set; }

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(
            nameof(ViewModel),
            typeof(OrderKanbanColumnViewModel),
            typeof(OrderKanbanColumnControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(OrderKanbanColumnControl),
            new PropertyMetadata("Pending"));

    public OrderKanbanColumnControl()
    {
        this.InitializeComponent();
    }
}