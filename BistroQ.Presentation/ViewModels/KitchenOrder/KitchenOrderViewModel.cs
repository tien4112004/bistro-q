using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Enums;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;

namespace BistroQ.Presentation.ViewModels.KitchenOrder;

public partial class KitchenOrderViewModel :
    ObservableObject,
    INavigationAware,
    IRecipient<CustomListViewSelectionChangedMessage<KitchenOrderItemViewModel>>,
    IRecipient<KitchenActionMessage>,
    IDisposable
{
    public OrderKanbanColumnViewModel PendingColumnVM { get; set; }
    public OrderKanbanColumnViewModel ProgressColumnVM { get; set; }

    public KitchenOrderButtonsViewModel KitchenOrderButtonsVM { get; set; }

    private readonly IOrderItemDataService _orderItemDataService;

    private readonly IMessenger _messenger;

    [ObservableProperty]
    private List<KitchenOrderItemViewModel> _selectedItems = new();

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
        ProgressColumnVM.ColumnType = KitchenColumnType.InProgress;
        ProgressColumnVM.LoadItems(OrderItemStatus.InProgress);
    }

    public void Receive(CustomListViewSelectionChangedMessage<KitchenOrderItemViewModel> message)
    {

        SelectedItems.Clear();
        SelectedItems.AddRange(PendingColumnVM.SelectedItems);
        SelectedItems.AddRange(ProgressColumnVM.SelectedItems);
        KitchenOrderButtonsVM.UpdateStates(SelectedItems);
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

    private async void Complete(IEnumerable<KitchenOrderItemViewModel> orderItems)
    {
        try
        {
            await Task.WhenAll(
                orderItems.Select(i => _orderItemDataService.UpdateOrderItemStatusAsync(i.OrderItemId, OrderItemStatus.Completed)));

            foreach (var item in orderItems)
            {
                item.Status = OrderItemStatus.Completed;
                ProgressColumnVM.Items.Remove(item);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private async void Move(IEnumerable<KitchenOrderItemViewModel> orderItems)
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
                    PendingColumnVM.Items.Remove(item);
                    ProgressColumnVM.Items.Add(item);
                }
                else
                {
                    ProgressColumnVM.Items.Remove(item);
                    PendingColumnVM.Items.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private async void Cancel(IEnumerable<KitchenOrderItemViewModel> orderItems)
    {
        try
        {
            await Task.WhenAll(
                orderItems.Select(i => _orderItemDataService.UpdateOrderItemStatusAsync(i.OrderItemId, OrderItemStatus.Cancelled)));

            foreach (var item in orderItems)
            {
                item.Status = OrderItemStatus.Cancelled;
                PendingColumnVM.Items.Remove(item);
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