using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Enums;
using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.KitchenOrder.Strategies;
using BistroQ.Presentation.ViewModels.Models;
using BistroQ.Presentation.ViewModels.States;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;

namespace BistroQ.Presentation.ViewModels.KitchenOrder;

/// <summary>
/// ViewModel responsible for managing the kitchen order interface.
/// Coordinates between pending and in-progress order columns, action buttons,
/// and handles order status transitions using strategy pattern.
/// </summary>
/// <remarks>
/// Implements:
/// - ObservableObject for MVVM pattern
/// - INavigationAware for navigation lifecycle
/// - IRecipient interfaces for handling selection and action messages
/// - IDisposable for resource cleanup
/// </remarks>
public partial class KitchenOrderViewModel :
    ObservableObject,
    INavigationAware,
    IRecipient<CustomListViewSelectionChangedMessage<OrderItemViewModel>>,
    IRecipient<KitchenActionMessage>,
    IDisposable
{
    #region Public Properties
    /// <summary>
    /// ViewModel for managing the pending orders column
    /// </summary>
    public OrderKanbanColumnViewModel PendingColumnVM { get; set; }

    /// <summary>
    /// ViewModel for managing the in-progress orders column
    /// </summary>
    public OrderKanbanColumnViewModel ProgressColumnVM { get; set; }

    /// <summary>
    /// ViewModel for managing kitchen order action buttons
    /// </summary>
    public KitchenOrderButtonsViewModel KitchenOrderButtonsVM { get; set; }
    #endregion

    #region Private Fields
    /// <summary>
    /// Service for order item data operations
    /// </summary>
    private readonly IOrderItemDataService _orderItemDataService;

    /// <summary>
    /// Service for dialog operations
    /// </summary>
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Messenger for inter-component communication
    /// </summary>
    private readonly IMessenger _messenger;

    /// <summary>
    /// Factory for creating kitchen action strategies
    /// </summary>
    private readonly KitchenStrategyFactory _strategyFactory;
    #endregion

    #region Observable Properties
    /// <summary>
    /// Gets or sets the state container for kitchen orders
    /// </summary>
    [ObservableProperty]
    private KitchenOrderState _state = new();
    #endregion

    #region Constructor
    public KitchenOrderViewModel(IOrderItemDataService orderItemDataService,
        IMessenger messenger,
        IDialogService dialogService,
        KitchenStrategyFactory strategyFactory,
        OrderKanbanColumnViewModel pendingColumnVM,
        OrderKanbanColumnViewModel progressColumnVM,
        KitchenOrderButtonsViewModel kitchenOrderButtonsViewModel)
    {
        PendingColumnVM = pendingColumnVM;
        ProgressColumnVM = progressColumnVM;
        _dialogService = dialogService;
        KitchenOrderButtonsVM = kitchenOrderButtonsViewModel;
        _strategyFactory = strategyFactory;
        _orderItemDataService = orderItemDataService;
        _messenger = messenger;
        _messenger.RegisterAll(this);
    }
    #endregion

    #region Navigation Methods
    /// <summary>
    /// Handles cleanup when navigating away from this view
    /// </summary>
    /// <returns>A completed task</returns>
    public Task OnNavigatedFrom()
    {
        Dispose();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Initializes columns and loads order items when navigating to this view
    /// </summary>
    /// <param name="parameter">Navigation parameter</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task OnNavigatedTo(object parameter)
    {
        PendingColumnVM.ColumnType = KitchenColumnType.Pending;
        await PendingColumnVM.LoadItems(OrderItemStatus.Pending);
        State.PendingItems = PendingColumnVM.Items;

        ProgressColumnVM.ColumnType = KitchenColumnType.InProgress;
        await ProgressColumnVM.LoadItems(OrderItemStatus.InProgress);
        State.ProgressItems = ProgressColumnVM.Items;
    }
    #endregion

    #region Message Handlers
    /// <summary>
    /// Handles selection changes in the order item lists
    /// </summary>
    /// <param name="message">Message containing selection change information</param>
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

    /// <summary>
    /// Handles kitchen action requests using strategy pattern
    /// </summary>
    /// <remarks>
    /// Executes the appropriate strategy with a minimum delay of 200ms
    /// to prevent UI flickering. Handles errors and resets button states.
    /// </remarks>
    /// <param name="message">Message containing the requested action</param>
    public async void Receive(KitchenActionMessage message)
    {
        try
        {
            var strategy = _strategyFactory.GetStrategy(message.Action, State);
            await TaskHelper.WithMinimumDelay(strategy.ExecuteAsync(State.SelectedItems), 200);
        }
        catch (Exception ex)
        {
            _ = _dialogService.ShowErrorDialog(ex.Message, "Error");
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            KitchenOrderButtonsVM.ResetLoadingState();
        }
    }
    #endregion

    #region IDisposable Implementation
    /// <summary>
    /// Performs cleanup of resources used by the ViewModel and its child ViewModels
    /// </summary>
    public void Dispose()
    {
        KitchenOrderButtonsVM.Dispose();
        PendingColumnVM.Dispose();
        ProgressColumnVM.Dispose();
        _messenger.UnregisterAll(this);
    }
    #endregion
}