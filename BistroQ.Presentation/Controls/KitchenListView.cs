using BistroQ.Presentation.ViewModels.Models;

namespace BistroQ.Presentation.Controls;

/// <summary>
/// Specialized version of CustomListView for handling OrderItemViewModel items.
/// Used specifically for kitchen order management interface.
/// </summary>
public class KitchenListView : CustomListView<OrderItemViewModel> { }