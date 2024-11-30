using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.Models;
using BistroQ.Presentation.ViewModels.KitchenOrder;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;

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

    private void ListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
    {
        if (e.Items.Count > 0)
        {
            var items = e.Items.Select(i => i as KitchenOrderItemViewModel);

            if (ViewModel.HasSelectedItems)
            {
                items = ViewModel.SelectedItems;
            }

            var dragData = new OrderItemDragData
            {
                OrderItems = items,
                SourceColumn = ViewModel.ColumnType
            };

            e.Data.Properties.Add("DraggedOrder", dragData);
            e.Data.RequestedOperation = DataPackageOperation.Move;
        }
    }

    private void ListView_DragOver(object sender, DragEventArgs e)
    {
        if (e.DataView.Properties.TryGetValue("DraggedOrder", out var dragDataObj))
        {
            e.AcceptedOperation = DataPackageOperation.Move;
            e.DragUIOverride.Caption = "Move order to " + Title;
        }
    }

    private void ListView_Drop(object sender, DragEventArgs e)
    {
        var listView = sender as ListView;
        var position = e.GetPosition(listView);

        // Calculate insert index based on Y position
        var insertIndex = listView.Items.Count;
        for (int i = 0; i < listView.Items.Count; i++)
        {
            var item = listView.ContainerFromIndex(i) as ListViewItem;
            if (item != null)
            {
                var itemBottom = item.TransformToVisual(listView)
                                    .TransformPoint(new Point(0, item.ActualHeight));
                if (position.Y < itemBottom.Y)
                {
                    insertIndex = i;
                    break;
                }
            }
        }

        if (e.DataView.Properties.TryGetValue("DraggedOrder", out var dragDataObj))
        {
            var dragData = dragDataObj as OrderItemDragData;

            ViewModel.HandleItemDroppedAsync(dragData.OrderItems, dragData.SourceColumn, insertIndex);
        }
    }

    private void CustomListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListView listView || listView.Tag is not string title || !e.AddedItems.Any())
            return;

        var messenger = App.GetService<IMessenger>();

        // Map of column titles and their corresponding columns to clear
        var columnsToClear = new Dictionary<string, string>
        {
            { "Pending", "In Progress" },
            { "In Progress", "Pending" }
        };

        if (columnsToClear.TryGetValue(title, out var columnToClear))
        {
            messenger.Send(new ChangeCustomListViewSelectionMessage<KitchenOrderItemViewModel>(new ObservableCollection<KitchenOrderItemViewModel>(), columnToClear));
        }
    }
}