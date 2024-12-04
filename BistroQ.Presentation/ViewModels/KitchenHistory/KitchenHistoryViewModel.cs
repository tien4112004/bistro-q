using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Enums;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.KitchenHistory;

public partial class KitchenHistoryViewModel : ObservableObject, INavigationAware
{
    [ObservableProperty]
    private ObservableCollection<OrderItemViewModel>? _items;

    private readonly IMapper _mapper;
    private readonly IOrderItemDataService _orderItemDataService;

    public ICommand LoadItemsCommand { get; }

    public KitchenHistoryViewModel(IMapper mapper, IOrderItemDataService orderItemDataService)
    {
        _mapper = mapper;
        _orderItemDataService = orderItemDataService;

        LoadItemsCommand = new AsyncRelayCommand<OrderItemStatus>(LoadItemsAsync);
    }

    private async Task LoadItemsAsync(OrderItemStatus status)
    {
        var orderItems = await _orderItemDataService.GetOrderItemsByStatusAsync(status);

        Items = new ObservableCollection<OrderItemViewModel>(orderItems.Select(_mapper.Map<OrderItemViewModel>));
    }

    public void OnNavigatedTo(object parameter)
    {
        //
    }

    public void OnNavigatedFrom()
    {
        //
    }
}
