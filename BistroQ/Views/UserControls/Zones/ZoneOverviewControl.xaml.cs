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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Views.UserControls.Zones;

public sealed partial class ZoneOverviewControl : UserControl
{
    public ZoneOverviewControl()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<ZoneOverviewViewModel>();
        Loaded += UserControl_Loaded;
    }

    public ZoneOverviewViewModel ViewModel { get; }

    public static readonly DependencyProperty ViewModelProperty =
    DependencyProperty.Register(
        nameof(ViewModel),
        typeof(ZoneOverviewViewModel),
        typeof(ZoneOverviewControl),
        new PropertyMetadata(null));


    public event EventHandler<int?> ZoneSelectionChanged;

    public event EventHandler<string> TypeSelectionChanged;

    private void OnZoneClicked(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is ZoneDto zone)
        {
            ZoneSelectionChanged?.Invoke(this, zone.ZoneId);
        }
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.InitializeAsync();
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

    private void Segmented_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        switch (Segmented.SelectedIndex)
        {
            case 0:
                TypeSelectionChanged?.Invoke(this, "All");
                break;
            case 1:
                TypeSelectionChanged?.Invoke(this, "Occupied");
                break;
            default:
                TypeSelectionChanged?.Invoke(this, "All");
                break;
        }
    }
}
