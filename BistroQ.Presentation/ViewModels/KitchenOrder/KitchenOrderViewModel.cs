using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Enums;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Models;
using BistroQ.Presentation.ViewModels.States;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;

namespace BistroQ.Presentation.ViewModels.KitchenOrder;

public partial class KitchenOrderViewModel :
    ObservableObject,
    INavigationAware,
    IRecipient<CustomListViewSelectionChangedMessage<OrderItemViewModel>>,
    IRecipient<KitchenActionMessage>,
    IDisposable
{
    public OrderKanbanColumnViewModel PendingColumnVM { get; set; }
    public OrderKanbanColumnViewModel ProgressColumnVM { get; set; }

    public KitchenOrderButtonsViewModel KitchenOrderButtonsVM { get; set; }

    private readonly IOrderItemDataService _orderItemDataService;

    private readonly IMessenger _messenger;

    [ObservableProperty]
    private List<OrderItemViewModel> _selectedItems = new();

    [ObservableProperty]
    private KitchenOrderState _state = new();

    public KitchenOrderViewModel(IOrderItemDataService orderItemDataService, IMessenger messenger, KitchenOrderButtonsViewModel kitchenOrderButtonsViewModel)
    {
        PendingColumnVM = App.GetService<OrderKanbanColumnViewModel>();
        ProgressColumnVM = App.GetService<OrderKanbanColumnViewModel>();
        KitchenOrderButtonsVM = kitchenOrderButtonsViewModel;
        _orderItemDataService = orderItemDataService;
        _messenger = messenger;
        _messenger.RegisterAll(this);
    }

    public void OnNavigatedFrom()
    {
        _messenger.UnregisterAll(this);
        PendingColumnVM.Dispose();
        ProgressColumnVM.Dispose();
        KitchenOrderButtonsVM.Dispose();
    }

    public void OnNavigatedTo(object parameter)
    {
        PendingColumnVM.ColumnType = KitchenColumnType.Pending;
        PendingColumnVM.LoadItems(OrderItemStatus.Pending);
        State.PendingItems = PendingColumnVM.Items;

        ProgressColumnVM.ColumnType = KitchenColumnType.InProgress;
        ProgressColumnVM.LoadItems(OrderItemStatus.InProgress);
        State.ProgressItems = ProgressColumnVM.Items;
    }

    public void Receive(CustomListViewSelectionChangedMessage<OrderItemViewModel> message)
    {
        State.SelectedItems.Clear();
        foreach (var item in PendingColumnVM.SelectedItems)
        {
            State.SelectedItems.Add(item);
        }
        foreach (var item in ProgressColumnVM.SelectedItems)
        {
            State.SelectedItems.Add(item);
        }
        KitchenOrderButtonsVM.UpdateStates(State.SelectedItems);
    }

    public void Receive(KitchenActionMessage message)
    {
        switch (message.Action)
        {
            case KitchenAction.Complete:
                Complete(message.OrderItems);
                break;
            case KitchenAction.Move:
                Move(message.OrderItems);
                break;
            case KitchenAction.Cancel:
                Cancel(message.OrderItems);
                break;
        }
    }

    private async void Complete(IEnumerable<OrderItemViewModel> orderItems)
    {
        try
        {
            await Task.WhenAll(
                orderItems.Select(i => _orderItemDataService.UpdateOrderItemStatusAsync(i.OrderItemId, OrderItemStatus.Completed)));

            foreach (var item in orderItems)
            {
                item.Status = OrderItemStatus.Completed;
                State.ProgressItems.Remove(item);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private async void Move(IEnumerable<OrderItemViewModel> orderItems)
    {
        try
        {
            var targetStatus = orderItems.First().Status == OrderItemStatus.Pending
                ? OrderItemStatus.InProgress
                : OrderItemStatus.Pending;
            await Task.WhenAll(
                orderItems.Select(i => _orderItemDataService.UpdateOrderItemStatusAsync(i.OrderItemId, targetStatus)));


            foreach (var item in orderItems)
            {
                item.Status = targetStatus;
                if (targetStatus == OrderItemStatus.InProgress)
                {
                    State.PendingItems.Remove(item);
                    State.ProgressItems.Add(item);
                }
                else
                {
                    State.ProgressItems.Remove(item);
                    State.PendingItems.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private async void Cancel(IEnumerable<OrderItemViewModel> orderItems)
    {
        try
        {
            await Task.WhenAll(
                orderItems.Select(i => _orderItemDataService.UpdateOrderItemStatusAsync(i.OrderItemId, OrderItemStatus.Cancelled)));

            foreach (var item in orderItems)
            {
                item.Status = OrderItemStatus.Cancelled;
                State.PendingItems.Remove(item);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    public void Dispose()
    {
        _messenger.UnregisterAll(this);
    }
}