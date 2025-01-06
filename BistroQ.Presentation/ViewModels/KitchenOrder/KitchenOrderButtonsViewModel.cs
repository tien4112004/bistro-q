using BistroQ.Domain.Enums;
using BistroQ.Presentation.Enums;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.KitchenOrder;

/// <summary>
/// ViewModel responsible for managing kitchen order action buttons and their states.
/// Handles completion, movement, and cancellation of orders with appropriate state management.
/// </summary>
/// <remarks>
/// Implements:
/// - ObservableObject for MVVM pattern
/// - IDisposable for resource cleanup
/// </remarks>
public partial class KitchenOrderButtonsViewModel : ObservableObject, IDisposable
{
    #region Private Fields
    /// <summary>
    /// Messenger service for handling inter-component communication
    /// </summary>
    private readonly IMessenger _messenger;
    #endregion

    #region Public Properties
    /// <summary>
    /// Collection of order items being managed
    /// </summary>
    public ObservableCollection<OrderItemViewModel> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets whether the complete action can be performed
    /// </summary>
    [ObservableProperty]
    private bool _canComplete;

    /// <summary>
    /// Gets or sets whether the move action can be performed
    /// </summary>
    [ObservableProperty]
    private bool _canMove;

    /// <summary>
    /// Gets or sets whether the cancel action can be performed
    /// </summary>
    [ObservableProperty]
    private bool _canCancel;

    /// <summary>
    /// Gets or sets whether the complete action is in progress
    /// </summary>
    [ObservableProperty]
    private bool _isCompleteLoading;

    /// <summary>
    /// Gets or sets whether the move action is in progress
    /// </summary>
    [ObservableProperty]
    private bool _isMoveLoading;

    /// <summary>
    /// Gets or sets whether the cancel action is in progress
    /// </summary>
    [ObservableProperty]
    private bool _isCancelLoading;
    #endregion

    #region Constructor
    public KitchenOrderButtonsViewModel(IMessenger messenger)
    {
        _messenger = messenger;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Updates the button states based on the provided order items
    /// </summary>
    /// <remarks>
    /// - Complete action is available for items in progress
    /// - Move action is available when there are any items
    /// - Cancel action is available for pending items
    /// </remarks>
    /// <param name="items">Collection of order items to evaluate</param>
    public void UpdateStates(IEnumerable<OrderItemViewModel> items)
    {
        Items = new ObservableCollection<OrderItemViewModel>(items);
        CanComplete = Items.Any(item => item.Status == OrderItemStatus.InProgress);
        CanMove = Items.Any();
        CanCancel = Items.Any(item => item.Status == OrderItemStatus.Pending);
    }

    /// <summary>
    /// Resets all loading state flags to false
    /// </summary>
    public void ResetLoadingState()
    {
        IsCompleteLoading = false;
        IsMoveLoading = false;
        IsCancelLoading = false;
    }

    /// <summary>
    /// Performs cleanup of resources used by the ViewModel
    /// </summary>
    public void Dispose()
    {
        _messenger.UnregisterAll(this);
    }
    #endregion

    #region Command Methods
    /// <summary>
    /// Handles the complete action for orders in progress
    /// </summary>
    [RelayCommand]
    private void Complete()
    {
        if (CanComplete)
        {
            CanComplete = false;
            CanMove = false;
            CanCancel = false;
            IsCompleteLoading = true;
            _messenger.Send(new KitchenActionMessage(KitchenAction.Complete));
        }
    }

    /// <summary>
    /// Handles the move action for orders
    /// </summary>
    [RelayCommand]
    private void Move()
    {
        if (CanMove)
        {
            CanComplete = false;
            CanMove = false;
            CanCancel = false;
            IsMoveLoading = true;
            _messenger.Send(new KitchenActionMessage(KitchenAction.Move));
        }
    }

    /// <summary>
    /// Handles the cancel action for pending orders
    /// </summary>
    [RelayCommand]
    private void Cancel()
    {
        if (CanCancel)
        {
            CanComplete = false;
            CanMove = false;
            CanCancel = false;
            IsCancelLoading = true;
            _messenger.Send(new KitchenActionMessage(KitchenAction.Cancel));
        }
    }
    #endregion
}