using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.States;

public partial class KitchenOrderState : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<OrderItemViewModel> _pendingItems = new();

    [ObservableProperty]
    private ObservableCollection<OrderItemViewModel> _progressItems = new();

    [ObservableProperty]
    private ObservableCollection<OrderItemViewModel> _selectedItems = new();

}
