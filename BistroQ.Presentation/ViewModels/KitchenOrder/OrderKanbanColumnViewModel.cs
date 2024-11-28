using System.Collections.ObjectModel;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.KitchenOrder;

public partial class OrderKanbanColumnViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<OrderItemViewModel> _items = new();
    
    public OrderKanbanColumnViewModel()
    {
        
    }
}