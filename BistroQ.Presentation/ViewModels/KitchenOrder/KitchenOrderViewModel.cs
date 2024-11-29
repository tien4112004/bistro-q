using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;

namespace BistroQ.Presentation.ViewModels.KitchenOrder;

public partial class KitchenOrderViewModel : ObservableObject, INavigationAware
{
    public OrderKanbanColumnViewModel PendingColumnVM { get; set; }
    public OrderKanbanColumnViewModel ProgressColumnVM { get; set; }
    
    private readonly IOrderItemDataService _orderItemDataService;
    
    public KitchenOrderViewModel(IOrderItemDataService orderItemDataService)
    {
        PendingColumnVM = App.GetService<OrderKanbanColumnViewModel>();
        ProgressColumnVM = App.GetService<OrderKanbanColumnViewModel>();
        _orderItemDataService = orderItemDataService;
    }

    public void OnNavigatedFrom()
    {

    }

    public void OnNavigatedTo(object parameter)
    {
        PendingColumnVM.LoadItems(OrderItemStatus.Pending);
        ProgressColumnVM.LoadItems(OrderItemStatus.InProgress);
    }
}