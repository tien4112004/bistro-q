using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.States;

/// <summary>
/// State management class for kitchen order operations.
/// Manages collections of orders in different states.
/// </summary>
public partial class KitchenOrderState : ObservableObject
{
    /// <summary>
    /// Gets or sets the collection of pending order items
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<OrderItemViewModel> _pendingItems = new();

    /// <summary>
    /// Gets or sets the collection of in-progress order items
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<OrderItemViewModel> _progressItems = new();

    /// <summary>
    /// Gets or sets the collection of selected order items
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<OrderItemViewModel> _selectedItems = new();
}