using BistroQ.Domain.Dtos.Orders;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.States;

public partial class OrderItemGridState : ObservableObject
{
    [ObservableProperty]
    private OrderItemColletionQueryParams _query = new();

    [ObservableProperty]
    private bool _isLoading;

    public ObservableCollection<OrderItemViewModel> Items { get; set; } = new();

    public void Reset()
    {
        Items.Clear();
        IsLoading = false;
        Query.Status = nameof(OrderItemStatus.Pending);
    }

    public void ReturnToFirstPage()
    {
        Query.Page = 1;
    }
}
