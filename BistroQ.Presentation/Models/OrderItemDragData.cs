using BistroQ.Presentation.Enums;
using BistroQ.Presentation.ViewModels.Models;

namespace BistroQ.Presentation.Models;

/// <summary>
/// Represents data associated with drag and drop operations for order items in the kitchen interface.
/// Used to track order items being dragged between kitchen columns.
/// </summary>
public class OrderItemDragData
{
    #region Public Properties
    /// <summary>
    /// Gets or sets the collection of order items being dragged.
    /// Contains the order items selected for the drag operation.
    /// </summary>
    public IEnumerable<OrderItemViewModel> OrderItems { get; set; }

    /// <summary>
    /// Gets or sets the type of kitchen column from which the drag operation originated.
    /// Identifies the source column (e.g., New, In Progress, Ready) of the dragged items.
    /// </summary>
    public KitchenColumnType SourceColumn { get; set; }
    #endregion
}