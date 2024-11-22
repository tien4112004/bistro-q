using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using BistroQ.Core.Dtos.Zones;
using BistroQ.Core.Contracts.Services;
using BistroQ.ViewModels.CashierTable;
using System.Diagnostics;
using CommunityToolkit.WinUI.Controls;
using BistroQ.Models;
using System.Text.Json;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

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
