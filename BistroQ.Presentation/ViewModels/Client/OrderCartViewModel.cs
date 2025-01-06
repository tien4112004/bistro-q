using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Domain.Models.Entities;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.Client;

/// <summary>
/// ViewModel responsible for managing the order cart functionality in the client interface.
/// Handles order creation, modification, and status tracking with support for nutrition facts.
/// </summary>
/// <remarks>
/// Implements:
/// - ObservableRecipient for MVVM pattern
/// - IRecipient for handling cart and order messages
/// - IDisposable for resource cleanup
/// </remarks>
public partial class OrderCartViewModel :
    ObservableRecipient,
    IRecipient<AddProductToCartMessage>,
    IRecipient<OrderRequestedMessage>,
    IDisposable
{
    #region Private Fields
    /// <summary>
    /// Service for handling order data operations
    /// </summary>
    private readonly IOrderDataService _orderDataService;

    /// <summary>
    /// AutoMapper instance for object mapping
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Messenger instance for handling inter-component communication
    /// </summary>
    private readonly IMessenger _messenger;

    /// <summary>
    /// Dispatcher queue for UI thread operations
    /// </summary>
    private DispatcherQueue dispatcherQueue;
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets or sets the current order being managed
    /// </summary>
    public OrderViewModel Order { get; set; }

    /// <summary>
    /// Gets or sets whether an order is currently being processed
    /// </summary>
    [ObservableProperty]
    private bool _isOrdering;

    /// <summary>
    /// Gets or sets whether the view model is loading data
    /// </summary>
    [ObservableProperty]
    private bool _isLoading = false;

    /// <summary>
    /// Gets or sets the current error message
    /// </summary>
    [ObservableProperty]
    private string _errorMessage = string.Empty;
    #endregion

    #region Nutrition Facts Properties
    /// <summary>
    /// Gets the calculated calories limit based on daily recommended value and people count
    /// </summary>
    public decimal CaloriesLimit => Math.Round(NutritionalConstants.DAILY_CALORIES / 3m * Order.PeopleCount, 0);

    /// <summary>
    /// Gets the calculated protein limit based on daily recommended value and people count
    /// </summary>
    public decimal ProteinLimit => Math.Round(NutritionalConstants.DAILY_PROTEIN / 3m * Order.PeopleCount, 0);

    /// <summary>
    /// Gets the calculated fat limit based on daily recommended value and people count
    /// </summary>
    public decimal FatLimit => Math.Round(NutritionalConstants.DAILY_FAT / 3m * Order.PeopleCount, 0);

    /// <summary>
    /// Gets the calculated fiber limit based on daily recommended value and people count
    /// </summary>
    public decimal FiberLimit => Math.Round(NutritionalConstants.DAILY_FIBER / 3m * Order.PeopleCount, 0);

    /// <summary>
    /// Gets the calculated carbohydrates limit based on daily recommended value and people count
    /// </summary>
    public decimal CarbohydratesLimit => Math.Round(NutritionalConstants.DAILY_CARBOHYDRATES / 3m * Order.PeopleCount, 0);

    /// <summary>
    /// Gets the percentage of calories relative to the daily limit
    /// </summary>
    public double CaloriesPercentage => CalculatePercentage(Order.TotalCalories, CaloriesLimit);

    /// <summary>
    /// Gets the percentage of protein relative to the daily limit
    /// </summary>
    public double ProteinPercentage => CalculatePercentage(Order.TotalProtein, ProteinLimit);

    /// <summary>
    /// Gets the percentage of fat relative to the daily limit
    /// </summary>
    public double FatPercentage => CalculatePercentage(Order.TotalFat, FatLimit);

    /// <summary>
    /// Gets the percentage of fiber relative to the daily limit
    /// </summary>
    public double FiberPercentage => CalculatePercentage(Order.TotalFiber, FiberLimit);

    /// <summary>
    /// Gets the percentage of carbohydrates relative to the daily limit
    /// </summary>
    public double CarbohydratesPercentage => CalculatePercentage(Order.TotalCarbohydrates, CarbohydratesLimit);
    #endregion

    #region Collections and Commands
    /// <summary>
    /// Gets the total cost of items in the cart
    /// </summary>
    public decimal TotalCart => CartItems.Sum(x => x.Total.Value);

    /// <summary>
    /// Collection of items currently in the cart
    /// </summary>
    public ObservableCollection<OrderItemViewModel> CartItems { get; set; } = new ObservableCollection<OrderItemViewModel>();

    /// <summary>
    /// Collection of items currently being processed
    /// </summary>
    public ObservableCollection<OrderItemViewModel> ProcessingItems { get; set; } = new ObservableCollection<OrderItemViewModel>();

    /// <summary>
    /// Collection of completed order items
    /// </summary>
    public ObservableCollection<OrderItemViewModel> CompletedItems { get; set; } = new ObservableCollection<OrderItemViewModel>();

    /// <summary>
    /// Command to initiate a new order
    /// </summary>
    public ICommand StartOrderCommand { get; }

    /// <summary>
    /// Command to cancel the current order
    /// </summary>
    public ICommand CancelOrderCommand { get; }

    /// <summary>
    /// Command executed when an order is started
    /// </summary>
    public ICommand OrderStartedCommand { get; set; }

    /// <summary>
    /// Command to modify the number of people in the order
    /// </summary>
    public ICommand EditPeopleCountCommand { get; }
    #endregion

    #region Constructor
    public OrderCartViewModel(IOrderDataService orderDataService, IMapper mapper, IMessenger messenger)
    {
        _orderDataService = orderDataService;
        _mapper = mapper;
        _messenger = messenger;
        messenger.RegisterAll(this);
        StartOrderCommand = new AsyncRelayCommand(StartOrder);
        CancelOrderCommand = new RelayCommand(CancelOrder);
        dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        EditPeopleCountCommand = new AsyncRelayCommand(EditPeopleCount);

        CartItems.CollectionChanged += CartItems_CollectionChanged;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Loads an existing order from the data service
    /// </summary>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task LoadExistingOrderAsync()
    {
        IsLoading = true;
        var existingOrder = await Task.Run(async () =>
        {
            var order = await _orderDataService.GetOrderAsync();
            return _mapper.Map<OrderViewModel>(order);
        });

        IsLoading = false;

        if (existingOrder == null)
        {
            return;
        }
        Order = existingOrder;
        IsOrdering = true;

        SeparateOrdersByStatus();
    }

    /// <summary>
    /// Gets whether there are any items in processing state
    /// </summary>
    public bool IsProcessingItemsEmpty => !ProcessingItems.Any();

    /// <summary>
    /// Gets whether there are any completed items
    /// </summary>
    public bool IsCompletedItemsEmpty => !CompletedItems.Any();

    /// <summary>
    /// Handles receiving a message to add a product to the cart
    /// </summary>
    /// <param name="message">Message containing the product to add</param>
    public void Receive(AddProductToCartMessage message)
    {
        var product = message.Product;
        var existingOrderItem = CartItems.FirstOrDefault(od => od.ProductId == product.ProductId);
        if (existingOrderItem != null)
        {
            existingOrderItem.Quantity++;
        }
        else
        {
            CartItems.Add(new OrderItemViewModel
            {
                OrderId = Order.OrderId,
                ProductId = product.ProductId,
                Product = product,
                Quantity = 1,
                PriceAtPurchase = (product.DiscountPrice != 0 ? product.DiscountPrice : product.Price)
            });
        }
    }

    /// <summary>
    /// Handles receiving a message to process the order
    /// </summary>
    /// <param name="message">Order request message</param>
    public async void Receive(OrderRequestedMessage message)
    {
        try
        {
            var cart = CartItems.Select(item => _mapper.Map<OrderItem>(item)).ToList();

            await _orderDataService.CreateOrderItems(cart);
            dispatcherQueue.TryEnqueue(() =>
            {
                CartItems.Clear();
                _messenger.Send(new OrderSucceededMessage());
            });
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// Performs cleanup of resources used by the ViewModel
    /// </summary>
    public void Dispose()
    {
        CartItems.CollectionChanged -= CartItems_CollectionChanged;
        _messenger.UnregisterAll(this);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Initiates a new order
    /// </summary>
    private async Task StartOrder()
    {
        try
        {
            IsLoading = true;

            Order = await Task.Run(
                async () =>
                {
                    var orders = await _orderDataService.CreateOrderAsync();
                    return _mapper.Map<OrderViewModel>(orders);
                });
            IsOrdering = true;

            OrderStartedCommand?.Execute(Order);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Cancels the current order
    /// </summary>
    private void CancelOrder()
    {
        _orderDataService.DeleteOrderAsync();
        Order = null;

        ErrorMessage = string.Empty;
        IsOrdering = false;
    }

    /// <summary>
    /// Updates the number of people in the current order
    /// </summary>
    private async Task EditPeopleCount()
    {
        await _orderDataService.ChangePeopleCountAsync(Order.PeopleCount);
        LoadExistingOrderAsync();
        return;
    }

    /// <summary>
    /// Handles changes in the cart items collection
    /// </summary>
    private void CartItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (OrderItemViewModel item in e.NewItems)
            {
                item.PropertyChanged += OrderItem_PropertyChanged;
            }
        }

        if (e.OldItems != null)
        {
            foreach (OrderItemViewModel item in e.OldItems)
            {
                item.PropertyChanged -= OrderItem_PropertyChanged;
            }
        }

        OnPropertyChanged(nameof(TotalCart));
        SeparateOrdersByStatus();
    }

    /// <summary>
    /// Handles property changes in order items
    /// </summary>
    private void OrderItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(OrderItemViewModel.Total))
        {
            OnPropertyChanged(nameof(TotalCart));
        }
    }

    /// <summary>
    /// Separates order items by their current status
    /// </summary>
    private void SeparateOrdersByStatus()
    {
        ProcessingItems.Clear();
        CompletedItems.Clear();

        foreach (var item in Order.OrderItems)
        {
            if (item.Status == OrderItemStatus.InProgress || item.Status == OrderItemStatus.Pending)
            {
                ProcessingItems.Add(item);
            }
            else if (item.Status == OrderItemStatus.Completed)
            {
                CompletedItems.Add(item);
            }
        }

        OnPropertyChanged(nameof(IsProcessingItemsEmpty));
        OnPropertyChanged(nameof(IsCompletedItemsEmpty));
    }

    /// <summary>
    /// Calculates the percentage of a value relative to a limit
    /// </summary>
    /// <param name="value">The current value</param>
    /// <param name="limit">The maximum limit</param>
    /// <returns>The percentage as a double</returns>
    private double CalculatePercentage(decimal value, decimal limit)
    {
        if (limit == 0)
        {
            return 100.0;
        }
        return decimal.ToDouble(value / limit * 100);
    }
    #endregion
}

/// <summary>
/// Contains constant values for nutritional calculations and ratings
/// </summary>
public static class NutritionalConstants
{
    // Daily recommended values
    public const decimal DAILY_CALORIES = 2000m;
    public const decimal DAILY_PROTEIN = 75m;
    public const decimal DAILY_CARBOHYDRATES = 300m;
    public const decimal DAILY_FAT = 70m;
    public const decimal DAILY_FIBER = 25m;

    // Rating weights (must sum to 1.0)
    public const decimal PROTEIN_WEIGHT = 0.3m;
    public const decimal FAT_WEIGHT = 0.2m;
    public const decimal CALORIES_WEIGHT = 0.1m;
    public const decimal FIBER_WEIGHT = 0.2m;
    public const decimal CARB_WEIGHT = 0.2m;

    // Individual deficiency multipliers
    public const decimal PROTEIN_DEFICIENCY_MULTIPLIER = 1.0m;
    public const decimal FAT_DEFICIENCY_MULTIPLIER = 3.0m; // Fat is harmful in excess
    public const decimal CALORIES_DEFICIENCY_MULTIPLIER = 2.0m;
    public const decimal FIBER_DEFICIENCY_MULTIPLIER = 0.5m;
    public const decimal CARB_DEFICIENCY_MULTIPLIER = 2.0m;
}