using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.ViewModels.CashierTable;
using BistroQ.Presentation.ViewModels.Models;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

namespace BistroQ.Presentation.Views.UserControls.Zones;

public sealed partial class ZoneTableGridControl : UserControl
{
    public ZoneTableGridControl()
    {
        this.InitializeComponent();
    }

    public ZoneTableGridViewModel ViewModel { get; set; }

    public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(ZoneTableGridViewModel),
                typeof(ZoneTableGridControl),
                new PropertyMetadata(null));

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

    private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Hand);
    }

    private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Arrow);
    }

    private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        foreach (var item in e.AddedItems)
        {
            var container = myGridView.ContainerFromItem(item) as GridViewItem;
            container.BorderBrush = new SolidColorBrush(Colors.Blue);
            container.BorderThickness = new Thickness(2);
        }

        foreach (var item in e.RemovedItems)
        {
            var container = myGridView.ContainerFromItem(item) as GridViewItem;
            container.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }
    }
}

public class TableItemStyleSelector : StyleSelector
{
    public Style DefaultStyle { get; set; }
    public Style CheckingOutStyle { get; set; }
    public Style OccupiedStyle { get; set; }

    protected override Style SelectStyleCore(object item, DependencyObject container)
    {
        if (item is TableViewModel tableVM)
        {
            if (tableVM.IsCheckingOut)
            {
                return CheckingOutStyle;
            }
            if (tableVM.IsSpaceOccupied)
            {
                return OccupiedStyle;
            }
        }
        return DefaultStyle;
    }
}
