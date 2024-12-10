using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Enums;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.KitchenOrder.Strategies;
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

    private readonly KitchenStrategyFactory _strategyFactory;

    [ObservableProperty]
    private KitchenOrderState _state = new();

    public KitchenOrderViewModel(IOrderItemDataService orderItemDataService,
        IMessenger messenger,
        KitchenStrategyFactory strategyFactory,
        OrderKanbanColumnViewModel pendingColumnVM,
        OrderKanbanColumnViewModel progressColumnVM,
        KitchenOrderButtonsViewModel kitchenOrderButtonsViewModel)
    {
        PendingColumnVM = pendingColumnVM;
        ProgressColumnVM = progressColumnVM;
        KitchenOrderButtonsVM = kitchenOrderButtonsViewModel;
        _strategyFactory = strategyFactory;
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
        try
        {
            var strategy = _strategyFactory.GetStrategy(message.Action, State);
            strategy.ExecuteAsync(State.SelectedItems);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    public void Dispose()
    {
        KitchenOrderButtonsVM.Dispose();
        PendingColumnVM.Dispose();
        ProgressColumnVM.Dispose();
        _messenger.UnregisterAll(this);
    }
}