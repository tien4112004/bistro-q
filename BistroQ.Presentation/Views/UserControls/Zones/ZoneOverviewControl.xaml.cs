using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.ViewModels.CashierTable;
using CommunityToolkit.WinUI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Presentation.Views.UserControls.Zones;

public sealed partial class ZoneOverviewControl : UserControl
{
    public ZoneOverviewControl()
    {
        this.InitializeComponent();
    }

    public ZoneOverviewViewModel ViewModel { get; set; }

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(
            nameof(ViewModel),
            typeof(ZoneOverviewViewModel),
            typeof(ZoneOverviewControl),
            new PropertyMetadata(null));

    private void Segmented_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel == null)
        {
            return;
        }

        var type = Segmented.SelectedIndex switch
        {
            0 => "All",
            1 => "Occupied",
            _ => "All"
        };
        ViewModel.UpdateFilter(type);
    }
    private void ScrollViewer_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
    {
        const double SCROLL_SPEED = 1.25;
        var scrollViewer = (ScrollViewer)sender;
        scrollViewer.ChangeView(
            scrollViewer.HorizontalOffset - e.Delta.Translation.X * SCROLL_SPEED,
            null,
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
}
