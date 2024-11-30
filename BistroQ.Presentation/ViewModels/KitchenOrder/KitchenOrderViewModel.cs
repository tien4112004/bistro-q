﻿using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Enums;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using System.Text.Json;

namespace BistroQ.Presentation.ViewModels.KitchenOrder;

public partial class KitchenOrderViewModel :
    ObservableObject,
    INavigationAware,
    IRecipient<CustomListViewSelectionChangedMessage>,
    IRecipient<KitchenActionMessage>
{
    public OrderKanbanColumnViewModel PendingColumnVM { get; set; }
    public OrderKanbanColumnViewModel ProgressColumnVM { get; set; }

    private readonly IOrderItemDataService _orderItemDataService;

    private readonly IMessenger _messenger;

    [ObservableProperty]
    private List<KitchenOrderItemViewModel> _selectedItems = new();

    public KitchenOrderViewModel(IOrderItemDataService orderItemDataService, IMessenger messenger)
    {
        PendingColumnVM = App.GetService<OrderKanbanColumnViewModel>();
        ProgressColumnVM = App.GetService<OrderKanbanColumnViewModel>();
        _orderItemDataService = orderItemDataService;
        _messenger = messenger;
        messenger.RegisterAll(this);
    }

    public void OnNavigatedFrom()
    {

    }

    public void OnNavigatedTo(object parameter)
    {
        PendingColumnVM.ColumnType = KitchenColumnType.Pending;
        PendingColumnVM.LoadItems(OrderItemStatus.Pending);
        ProgressColumnVM.ColumnType = KitchenColumnType.InProgress;
        ProgressColumnVM.LoadItems(OrderItemStatus.InProgress);
    }

    public void Receive(CustomListViewSelectionChangedMessage message)
    {
        SelectedItems.Clear();
        SelectedItems.AddRange(PendingColumnVM.SelectedItems);
        SelectedItems.AddRange(ProgressColumnVM.SelectedItems);
    }

    public void Receive(KitchenActionMessage message)
    {
        Debug.WriteLine($"Received message: {message.Action}");
        Debug.WriteLine($"Selected items: " + JsonSerializer.Serialize(message.OrderItemIds));
    }
}