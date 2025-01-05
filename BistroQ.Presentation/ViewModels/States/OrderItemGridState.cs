using BistroQ.Domain.Dtos.Orders;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.States;

/// <summary>
/// State management class for order item grid display.
/// Handles query parameters and loading state for order items.
/// </summary>
public partial class OrderItemGridState : ObservableObject
{
    /// <summary>
    /// Gets or sets the query parameters for order item collection
    /// </summary>
    [ObservableProperty]
    private OrderItemColletionQueryParams _query = new();

    /// <summary>
    /// Gets or sets whether data is currently being loaded
    /// </summary>
    [ObservableProperty]
    private bool _isLoading;

    /// <summary>
    /// Gets or sets the collection of order items
    /// </summary>
    public ObservableCollection<OrderItemViewModel> Items { get; set; } = new();

    /// <summary>
    /// Resets the state to default values
    /// </summary>
    public void Reset()
    {
        Items.Clear();
        IsLoading = false;
        Query.Status = nameof(OrderItemStatus.Pending);
    }

    /// <summary>
    /// Returns to the first page of results
    /// </summary>
    public void ReturnToFirstPage()
    {
        Query.Page = 1;
    }
}