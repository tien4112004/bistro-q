using BistroQ.Core.Dtos.Zones;
using BistroQ.Models;
using BistroQ.ViewModels.CashierTable;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Views.UserControls.Zones;

public sealed partial class ZoneOverviewControl : UserControl
{
    public ZoneOverviewControl()
    {
        this.InitializeComponent();
    }

    public ZoneOverviewViewModel ViewModel { get; set; } = App.GetService<ZoneOverviewViewModel>();

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(
            nameof(ViewModel),
            typeof(ZoneOverviewViewModel),
            typeof(ZoneOverviewControl),
            new PropertyMetadata(null));

    private ZoneStateEventArgs _state = new();

    public event EventHandler<ZoneStateEventArgs> ZoneSelectionChanged;

    public event EventHandler<int?> TableSelectionChanged;

    private void OnZoneClicked(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is ZoneDto zone)
        {
            _state.ZoneId = zone.ZoneId;
            ZoneSelectionChanged?.Invoke(this, _state);
        }
    }

    private void Segmented_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _state.ZoneId = ViewModel.SelectedZone?.ZoneId;
        _state.Type = Segmented.SelectedIndex switch
        {
            0 => "All",
            1 => "Occupied",
            _ => "All"
        };

        ZoneSelectionChanged?.Invoke(this, _state);
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
}
