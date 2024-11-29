﻿using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.Models;
using BistroQ.Presentation.ViewModels.KitchenOrder;
using BistroQ.Presentation.ViewModels.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.ApplicationModel.DataTransfer;

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
        if (e.DataView.Properties.TryGetValue("DraggedOrder", out var dragDataObj))
        {
            var dragData = dragDataObj as OrderItemDragData;

            ViewModel.HandleItemDroppedAsync(dragData.OrderItems, dragData.SourceColumn);
        }
    }
}