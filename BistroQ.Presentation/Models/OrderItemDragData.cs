using BistroQ.Presentation.Enums;
using BistroQ.Presentation.ViewModels.Models;

namespace BistroQ.Presentation.Models;

public class OrderItemDragData
{
    public IEnumerable<KitchenOrderItemViewModel> OrderItems { get; set; }
    public KitchenColumnType SourceColumn { get; set; }
}