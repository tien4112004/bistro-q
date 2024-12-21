using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.Enums;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BistroQ.Presentation.ViewModels.KitchenOrder;

public partial class OrderKanbanColumnViewModel :
    ObservableObject,
    IRecipient<RemoveOrderItemsMessage>,
    IDisposable
{
    [ObservableProperty]
    private ObservableCollection<OrderItemViewModel> _items = new();

    public ObservableCollection<OrderItemViewModel> SelectedItems { get; set; } = new();

    public bool HasSelectedItems => SelectedItems.Any();

    public KitchenColumnType ColumnType { get; set; }

    private readonly IOrderItemDataService _orderItemDataService;
    private readonly IMapper _mapper;
    private readonly IMessenger _messenger;

    public OrderKanbanColumnViewModel(IOrderItemDataService orderItemDataService, IMapper mapper, IMessenger messenger)
    {
        _orderItemDataService = orderItemDataService;
        _mapper = mapper;
        _messenger = messenger;
        messenger.RegisterAll(this);
    }

    public async Task LoadItems(OrderItemStatus status)
    {
        try
        {
            var orderItems = await _orderItemDataService.GetOrderItemsByStatusAsync(status);
            var orderItemViewModels = _mapper.Map<IEnumerable<OrderItemViewModel>>(orderItems);
            Items = new ObservableCollection<OrderItemViewModel>(orderItemViewModels);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    public async Task HandleItemDroppedAsync(
        IEnumerable<OrderItemViewModel> items,
        KitchenColumnType sourceColumn,
        int insertIndex)
    {
        try
        {
            var targetStatus = ColumnType == KitchenColumnType.Pending ? OrderItemStatus.Pending : OrderItemStatus.InProgress;
            await _orderItemDataService.BulkUpdateOrderItemsStatusAsync(items.Select(i => i.OrderItemId), targetStatus);

            foreach (var item in items)
            {
                item.Status = targetStatus;
                Items.Insert(insertIndex, item);
                insertIndex++;
            }

            _messenger.Send(new RemoveOrderItemsMessage(items.Select(i => i.OrderItemId), sourceColumn));

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }


    public void Receive(RemoveOrderItemsMessage message)
    {
        if (message.Source != ColumnType) return;

        var itemsToRemove = Items
            .Where(i => message.OrderItemIds.Contains(i.OrderItemId))
            .ToList();

        foreach (var item in itemsToRemove)
        {
            Items.Remove(item);
        }
    }

    public void Dispose()
    {
        _messenger.UnregisterAll(this);
    }
}