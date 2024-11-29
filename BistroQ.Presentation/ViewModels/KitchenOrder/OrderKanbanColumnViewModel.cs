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
    IRecipient<RemoveOrderItemsMessage>
{
    [ObservableProperty]
    private ObservableCollection<KitchenOrderItemViewModel> _items = new();

    [ObservableProperty]
    private ObservableCollection<KitchenOrderItemViewModel> _selectedItems = new();

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

    public async void LoadItems(OrderItemStatus status)
    {
        try
        {
            var orderItems = await _orderItemDataService.GetOrderItemsByStatusAsync(status);
            var orderItemViewModels = _mapper.Map<IEnumerable<KitchenOrderItemViewModel>>(orderItems);
            Items = new ObservableCollection<KitchenOrderItemViewModel>(orderItemViewModels);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    public async void HandleItemDroppedAsync(IEnumerable<KitchenOrderItemViewModel> items, KitchenColumnType sourceColumn)
    {
        try
        {
            await Task.WhenAll(
                items.Select(i => _orderItemDataService.UpdateOrderItemStatusAsync(i.OrderItemId,
                    ColumnType == KitchenColumnType.Pending ? OrderItemStatus.Pending : OrderItemStatus.InProgress)));
            _messenger.Send(new RemoveOrderItemsMessage(items.Select(i => i.OrderItemId), sourceColumn));

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    public async void Receive(RemoveOrderItemsMessage message)
    {
        if (message.Source == ColumnType)
        {
            foreach (var itemId in message.OrderItemIds)
            {
                var item = Items.FirstOrDefault(i => i.OrderItemId == itemId);
                if (item != null)
                {
                    Items.Remove(item);
                }
            }
        }
    }
}