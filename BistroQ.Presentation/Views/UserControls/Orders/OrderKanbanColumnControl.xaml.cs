using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.ViewModels.KitchenOrder;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Presentation.Views.UserControls.Orders;

public sealed partial class OrderKanbanColumnControl : UserControl
{
    public OrderKanbanColumnViewModel ViewModel { get; set; }

    public string? Title { get; set; }

    public string? TitleIconPath { get; set; }

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(
            nameof(ViewModel),
            typeof(OrderKanbanColumnViewModel),
            typeof(OrderKanbanColumnControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty TitleIconPathProperty =
        DependencyProperty.Register(
            nameof(TitleIconPath),
            typeof(string),
            typeof(OrderKanbanColumnControl),
            new PropertyMetadata(""));

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

    private void ListViewItem_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        if (sender is UIElement element)
        {
            element.ChangeCursor(CursorType.Hand);
        }
    }

    private void ListViewItem_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        if (sender is UIElement element)
        {
            element.ChangeCursor(CursorType.Arrow);
        }
    }
}