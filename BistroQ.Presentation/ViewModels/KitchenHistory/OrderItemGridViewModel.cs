using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Models;
using BistroQ.Presentation.ViewModels.States;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BistroQ.Presentation.ViewModels.KitchenHistory;

/// <summary>
/// ViewModel responsible for managing the order item grid display and interactions.
/// Handles data loading, pagination, status filtering, and message-based updates.
/// </summary>
/// <remarks>
/// Implements:
/// - ObservableObject for MVVM pattern
/// - IDisposable for resource cleanup
/// - IRecipient interfaces for handling various message types
/// </remarks>
public partial class OrderItemGridViewModel :
    ObservableObject,
    IDisposable,
    IRecipient<OrderGridStatusChangedMessage>,
    IRecipient<PageSizeChangedMessage>,
    IRecipient<CurrentPageChangedMessage>
{
    #region Private Fields
    /// <summary>
    /// Messenger service for handling inter-component communication
    /// </summary>
    private readonly IMessenger _messenger;

    /// <summary>
    /// AutoMapper instance for object mapping
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Service for order item data operations
    /// </summary>
    private readonly IOrderItemDataService _dataService;

    /// <summary>
    /// Service for dialog display operations
    /// </summary>
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Flag indicating whether the view model has been disposed
    /// </summary>
    private bool _isDisposed;
    #endregion

    #region Observable Properties
    /// <summary>
    /// Gets or sets the state container for the order item grid
    /// </summary>
    [ObservableProperty]
    private OrderItemGridState _state = new();
    #endregion

    #region Constructor
    public OrderItemGridViewModel(
        IMessenger messenger,
        IMapper mapper,
        IDialogService dialogService,
        IOrderItemDataService dataService)
    {
        _messenger = messenger;
        _mapper = mapper;
        _dialogService = dialogService;
        _dataService = dataService;
        _messenger.RegisterAll(this);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Loads or reloads the order items with a minimum delay
    /// </summary>
    /// <remarks>
    /// Includes a 200ms minimum delay to prevent UI flickering during quick updates.
    /// Handles disposed object exceptions with detailed logging.
    /// </remarks>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task LoadItemsAsync()
    {
        if (State.IsLoading || _isDisposed) return;
        try
        {
            State.IsLoading = true;

            var result = await TaskHelper.WithMinimumDelay(_dataService.GetOrderItemsAsync(State.Query), 200);

            var items = _mapper.Map<IEnumerable<OrderItemViewModel>>(result.Data);
            State.Items = new ObservableCollection<OrderItemViewModel>(items);
            _messenger.Send(new PaginationChangedMessage(
                result.TotalItems,
                result.CurrentPage,
                result.TotalPages
             ));
        }
        catch (Exception ex)
        {
            if (ex is ObjectDisposedException e)
            {
                Debug.WriteLine(
                    "Disposed object details:\n" +
                    $"Object Name: {e.ObjectName}\n" +
                    $"Message: {e.Message}\n" +
                    $"Source: {e.Source}\n" +
                    $"Stack Trace: {e.StackTrace}"
                );
            }
            await _dialogService.ShowErrorDialog(ex.Message, "Error");
        }
        finally
        {
            State.IsLoading = false;
        }
    }

    /// <summary>
    /// Handles the receipt of a current page change message
    /// </summary>
    /// <param name="message">Message containing the new current page</param>
    public void Receive(CurrentPageChangedMessage message)
    {
        State.Query.Page = message.NewCurrentPage;
        _ = LoadItemsAsync();
    }

    /// <summary>
    /// Handles the receipt of an order grid status change message
    /// </summary>
    /// <param name="message">Message containing the new status</param>
    public void Receive(OrderGridStatusChangedMessage message)
    {
        State.Reset();
        State.Query.Status = message.Status.ToString();
        State.ReturnToFirstPage();
        _ = LoadItemsAsync();
    }

    /// <summary>
    /// Handles the receipt of a page size change message
    /// </summary>
    /// <param name="message">Message containing the new page size</param>
    public void Receive(PageSizeChangedMessage message)
    {
        State.Query.Size = message.NewPageSize;
        State.ReturnToFirstPage();
        _ = LoadItemsAsync();
    }

    /// <summary>
    /// Performs cleanup of resources used by the ViewModel
    /// </summary>
    public void Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;
        _messenger.UnregisterAll(this);
    }
    #endregion
}