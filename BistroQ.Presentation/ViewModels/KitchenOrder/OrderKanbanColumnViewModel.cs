using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BistroQ.Presentation.ViewModels.KitchenOrder;

public partial class OrderKanbanColumnViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<KitchenOrderItemViewModel> _items = new();

    private readonly IOrderItemDataService _orderItemDataService;
    private readonly IMapper _mapper;

    public OrderKanbanColumnViewModel(IOrderItemDataService orderItemDataService, IMapper mapper)
    {
        _orderItemDataService = orderItemDataService;
        _mapper = mapper;
    }

    public async void LoadItems(OrderItemStatus status)
    {
        try
        {
            var orderItems = await _orderItemDataService.GetOrderItemsByStatusAsync(status);
            var orderItemViewModels = _mapper.Map<IEnumerable<KitchenOrderItemViewModel>>(orderItems);
            Items = new ObservableCollection<KitchenOrderItemViewModel>(orderItemViewModels);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}