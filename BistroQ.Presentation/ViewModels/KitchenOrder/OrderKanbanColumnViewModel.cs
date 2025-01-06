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

/// <summary>
/// ViewModel responsible for managing a single column in the kitchen order kanban board.
/// Handles item loading, selection, drag-drop operations, and status updates.
/// </summary>
/// <remarks>
/// Implements:
/// - ObservableObject for MVVM pattern
/// - IRecipient for handling removal messages
/// - IDisposable for resource cleanup
/// </remarks>
public partial class OrderKanbanColumnViewModel :
    ObservableObject,
    IRecipient<RemoveOrderItemsMessage>,
    IDisposable
{
    #region Observable Collections
    /// <summary>
    /// Gets or sets the collection of order items in this column
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<OrderItemViewModel> _items = new();

    /// <summary>
    /// Gets or sets the collection of currently selected order items
    /// </summary>
    public ObservableCollection<OrderItemViewModel> SelectedItems { get; set; } = new();
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets whether there are any items currently selected
    /// </summary>
    public bool HasSelectedItems => SelectedItems.Any();

    /// <summary>
    /// Gets or sets the type of this kanban column (Pending or InProgress)
    /// </summary>
    public KitchenColumnType ColumnType { get; set; }
    #endregion

    #region Private Fields
    /// <summary>
    /// Service for order item data operations
    /// </summary>
    private readonly IOrderItemDataService _orderItemDataService;

    /// <summary>
    /// AutoMapper instance for object mapping
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Messenger for inter-component communication
    /// </summary>
    private readonly IMessenger _messenger;
    #endregion

    #region Constructor
    public OrderKanbanColumnViewModel(IOrderItemDataService orderItemDataService, IMapper mapper, IMessenger messenger)
    {
        _orderItemDataService = orderItemDataService;
        _mapper = mapper;
        _messenger = messenger;
        messenger.RegisterAll(this);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Loads order items with the specified status into this column
    /// </summary>
    /// <param name="status">The status of items to load</param>
    /// <returns>A task representing the asynchronous operation</returns>
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

    /// <summary>
    /// Handles items being dropped into this column from another column
    /// </summary>
    /// <remarks>
    /// Updates the status of dropped items and manages their insertion
    /// into the target column while removing them from the source column.
    /// </remarks>
    /// <param name="items">The items being dropped</param>
    /// <param name="sourceColumn">The column the items are coming from</param>
    /// <param name="insertIndex">The index where items should be inserted</param>
    /// <returns>A task representing the asynchronous operation</returns>
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

    /// <summary>
    /// Handles receiving a message to remove items from this column
    /// </summary>
    /// <param name="message">Message containing items to remove</param>
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

    /// <summary>
    /// Performs cleanup of resources used by the ViewModel
    /// </summary>
    public void Dispose()
    {
        _messenger.UnregisterAll(this);
    }
    #endregion
}