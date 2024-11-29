using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

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
        PendingColumnVM.ColumnType = KitchenColumnType.Pending;
        PendingColumnVM.LoadItems(OrderItemStatus.Pending);
        ProgressColumnVM.ColumnType = KitchenColumnType.InProgress;
        ProgressColumnVM.LoadItems(OrderItemStatus.InProgress);
    }
}